using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppOrderHistoryEntityTypeConfiguration : IEntityTypeConfiguration<AppOrderHistory>
    {
        public void Configure(EntityTypeBuilder<AppOrderHistory> builder)
        {
            builder.ToTable("OrderHistories");
            builder.Property(c => c.Item).HasMaxLength(5000).IsRequired();
            builder.Property(c => c.Total).IsRequired();
            builder.Property(c => c.Price).IsRequired();

        }
    }
}
