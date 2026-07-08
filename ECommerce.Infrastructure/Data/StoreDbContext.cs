using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
            
        }

        #region DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductsBrand> ProductsBrands { get; set; }
        public DbSet<ProductsType> ProductsTypes { get; set; }
        #endregion
        #region Catch Configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
        }
        #endregion
    }
}
