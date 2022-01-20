using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    class AppRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("Roles");
        }
    }
}
