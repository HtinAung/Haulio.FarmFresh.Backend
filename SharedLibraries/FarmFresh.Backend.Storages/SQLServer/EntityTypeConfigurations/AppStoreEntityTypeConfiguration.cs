using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppStoreEntityTypeConfiguration : IEntityTypeConfiguration<AppStore>
    {
        public void Configure(EntityTypeBuilder<AppStore> builder)
        {
            builder.ToTable("Stores");
            builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
            builder.HasMany(c => c.Products).WithOne(c => c.Store).HasForeignKey(c => c.StoreId);
            builder.HasMany(c => c.OrderHistories).WithOne(c => c.Store).HasForeignKey(c => c.StoreId);
            

        }
    }
}
