using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppIdentityUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<AppIdentityUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUserClaim> builder)
        {
            builder.ToTable("UserClaims");
        }
    }
}
