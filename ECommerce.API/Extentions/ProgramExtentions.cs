using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Extentions
{
    public static class ProgramExtentions
    {
        public static async Task MigrationAndSeedAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var Cataloglogger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogDataSeeder>>();

            var pending = await dbContext.Database.GetPendingMigrationsAsync();

            if (pending.Count() > 0)
            {
                await dbContext.Database.MigrateAsync();
            }

            CatalogDataSeeder catalogDataSeeder = new CatalogDataSeeder(dbContext, Cataloglogger);

            await catalogDataSeeder.SeedAsync();
        }
    }
}
