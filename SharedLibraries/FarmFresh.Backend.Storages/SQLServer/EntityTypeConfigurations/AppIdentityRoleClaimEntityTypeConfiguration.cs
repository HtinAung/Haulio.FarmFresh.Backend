using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppIdentityRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<AppIdentityRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AppIdentityRoleClaim> builder)
        {
            builder.ToTable("RoleClaims");
        }
    }
}
