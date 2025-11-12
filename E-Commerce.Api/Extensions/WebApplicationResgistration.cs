using E_Commerce.Domian.Interfaces;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Api.Extensions
{
    public static class WebApplicationResgistration
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();

            var storeDbContext = scope.ServiceProvider.GetService<StoreDbContext>();

            if ((await storeDbContext?.Database.GetPendingMigrationsAsync()!).Any())

                await storeDbContext.Database.MigrateAsync();

            return app;
        }
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();

            var DataInitializer = scope.ServiceProvider.GetService<IDataInitializer>();

            await DataInitializer?.InitializeAsync()!;

            return app;
        }
    }
}
