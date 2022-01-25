using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppProductEntityTypeConfiguration : IEntityTypeConfiguration<AppProduct>
    {
        public void Configure(EntityTypeBuilder<AppProduct> builder)
        {
            builder.ToTable("Products");
            builder.Property(c => c.Name).HasMaxLength(1000).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(5000).IsRequired(false);
            builder.Property(c => c.ImageUrl).HasMaxLength(5000).IsRequired(false);
            builder.Property(c => c.AvailableAmount).IsRequired();
            builder.Property(c => c.Price).IsRequired();
        }
    }
}
