using FarmFresh.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations
{
    public class AppIdentityUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppIdentityUserRole>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUserRole> builder)
        {
            builder.ToTable("UserRoles");
        }
    }
}
