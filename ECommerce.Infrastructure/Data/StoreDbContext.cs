using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Orders;
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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        #endregion

        #region Catch Configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
        }
        #endregion
    }
}
