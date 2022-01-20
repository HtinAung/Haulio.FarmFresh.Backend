using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    class AppProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<AppProductCategory>
    {
        public void Configure(EntityTypeBuilder<AppProductCategory> builder)
        {
            builder.ToTable("ProductCategories");
            builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
            builder.HasMany(c => c.Products).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId);
        }
    }
}
