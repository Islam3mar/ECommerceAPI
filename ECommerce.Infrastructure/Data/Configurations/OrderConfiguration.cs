using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal).HasColumnType("decimal(10,2)");

            builder.Property(o => o.BuyerEmail)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(o => o.status)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.OwnsOne(o => o.ShippingAddress);

          
        }
    }
}
