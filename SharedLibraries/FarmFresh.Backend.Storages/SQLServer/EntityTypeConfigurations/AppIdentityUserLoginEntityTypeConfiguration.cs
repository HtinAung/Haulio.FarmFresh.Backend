using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppIdentityUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<AppIdentityUserLogin>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUserLogin> builder)
        {
            builder.ToTable("UserLogins");
        }
    }
}
