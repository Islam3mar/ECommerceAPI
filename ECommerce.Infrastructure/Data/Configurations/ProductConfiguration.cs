using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PictureUrl).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);

            builder.Property(p => p.Price).HasColumnType("decimal(10,2)");


        }
    }
}
