using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d => d.Cost).HasColumnType("decimal(10,2)");
            builder.Property(d => d.ShortName).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(d => d.Description).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(d => d.DeliveryTime).HasColumnType("varchar").HasMaxLength(50);


        }
    }
}
