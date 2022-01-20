using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppIdentityUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<AppIdentityUserToken>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUserToken> builder)
        {
            builder.ToTable("UserTokens");
        }
    }
}
