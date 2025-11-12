
using E_Commerce.Domian.Interfaces;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.Data.Migrations;
using E_Commerce.Persistence.Data.SeedData;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Presentation.Api.Extensions;
using Microsoft.EntityFrameworkCore;
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
                options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbConnection"));
            });
            #endregion

            #region Services
            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(config =>
            {
                config.AddMaps(typeof(ProductModuleTables).Assembly);
            });
            #endregion

            #endregion



            var app = builder.Build();

            #region Data-Seed & Apply Migrations

            await app.MigrateDatabaseAsync();
            await app.SeedDataAsync();

            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
