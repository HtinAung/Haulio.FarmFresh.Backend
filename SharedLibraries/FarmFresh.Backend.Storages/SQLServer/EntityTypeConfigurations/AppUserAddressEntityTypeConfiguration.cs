using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppUserAddressEntityTypeConfiguration : IEntityTypeConfiguration<AppUserAddress>
    {
        public void Configure(EntityTypeBuilder<AppUserAddress> builder)
        {

            builder
                .Property(c => c.AddressLine)
                .IsRequired()
                .HasMaxLength(1000);

            builder
                .Property(c => c.City)
                .IsRequired()
                .HasMaxLength(500);

            builder
                .Property(c => c.Region)
                .IsRequired()
                .HasMaxLength(500);

            builder
                .Property(c => c.Country)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(c => c.PostalCode)
                .IsRequired()
                .HasMaxLength(6);

        }
    }
}
