using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Seeding
{
    public class CatalogDataSeeder(StoreDbContext storeDbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                var seedPath = Path.Combine(AppContext.BaseDirectory, "DataSeed"); // Folder Path

                await SeedIfEmptyAsync<ProductsBrand>(seedPath, "brands.json", ct);
                await SeedIfEmptyAsync<ProductsType>(seedPath, "types.json", ct);
                await SeedIfEmptyAsync<Product>(seedPath, "products.json", ct);

                
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Failed To Seed Data");
                throw;
            }
        }

        private async Task SeedIfEmptyAsync<T>(string root,string fileName,CancellationToken ct) where T : class
        {
            if (await storeDbContext.Set<T>().AnyAsync(ct)) return;
           
            var filePath = Path.Combine(root, fileName);

            if (!File.Exists(filePath))
            {
                logger.LogWarning($"Seed File Not Found: {filePath}");
                return;
            }

            await using var stream = File.OpenRead(filePath);

            var items = await JsonSerializer.DeserializeAsync<List<T>>(stream,new JsonSerializerOptions { PropertyNameCaseInsensitive = true } ,ct);

            if(items?.Count > 0)
                await storeDbContext.Set<T>().AddRangeAsync(items, ct);


            await storeDbContext.SaveChangesAsync(ct);

        }
    }
}
