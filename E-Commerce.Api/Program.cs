
using E_Commerce.Api.Factories;
using E_Commerce.Api.Middleware;
using E_Commerce.Domian.Entites.IdentityModule;
using E_Commerce.Domian.Interfaces;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.Data.Migrations;
using E_Commerce.Persistence.Data.SeedData;
using E_Commerce.Persistence.IdentityData.DbContexts;
using E_Commerce.Persistence.IdentityData.SeedData;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Presentation.Api.Extensions;
using E_Commerce.Service.Abstraction.Interfaces;
using E_Commerce.Service.Implementation.MappingProfiles.ProductModule;
using E_Commerce.Service.Implementation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            #region IoC

            #region Database
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbConnection"))
                //.UseLazyLoadingProxies()
                ;
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = ConfigurationOptions.Parse(
                    builder.Configuration.GetConnectionString("RedisConnection")!
                );

                options.Ssl = false; // ❗ مهم جدًا
                options.AbortOnConnectFail = false;

                return ConnectionMultiplexer.Connect(options);
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbConnection"));
            });
            builder.Services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            #endregion

            #region Services
            builder.Services.AddKeyedScoped<IDataInitializer, DataInitializer>("default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("identity");
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddAutoMapper(config =>
            {
                config.AddMaps(typeof(ProductModuleProfile).Assembly);
            });
            builder.Services.AddTransient<ProductPictureUrlResolver>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });

            #endregion

            #endregion

            #region Jwt token

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!)),
                    ClockSkew = System.TimeSpan.Zero
                };

            });

            #endregion



            var app = builder.Build();

            #region Data-Seed & Apply Migrations

            await app.MigrateDatabaseAsync<StoreDbContext>();
            await app.MigrateDatabaseAsync<StoreIdentityDbContext>();
            await app.SeedDataAsync("default");
            await app.SeedDataAsync("identity");

            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
