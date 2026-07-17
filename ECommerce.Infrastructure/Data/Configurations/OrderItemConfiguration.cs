using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Price)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.OwnsOne(oi => oi.product);
                   
        }
    }
}
