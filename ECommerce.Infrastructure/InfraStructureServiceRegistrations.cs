using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
    public static class InfraStructureServiceRegistrations
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("StoreDbConnection"));
            });

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog"); // Register the CatalogDataSeeder with a key "Catalog" Because later in another class he will need to Impelement The same interface
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
