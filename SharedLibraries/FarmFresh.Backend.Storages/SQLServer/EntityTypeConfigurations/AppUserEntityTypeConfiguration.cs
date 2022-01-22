using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");

            builder
                .Property(c => c.FullName)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .HasMany(c => c.Addresses)
                .WithOne(c => c.AppUser)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.OrderHistories)
                .WithOne(c => c.User).HasForeignKey(c => c.UserId);

            builder.HasOne(c => c.Store)
                .WithOne(c => c.User)
                .IsRequired(false)
                .HasForeignKey<AppStore>(c => c.UserId);
                
        }
    }
}
