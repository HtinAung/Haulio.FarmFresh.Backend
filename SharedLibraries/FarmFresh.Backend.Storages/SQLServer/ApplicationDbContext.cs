using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Storages.SQLServer.EntityTypeConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace FarmFresh.Backend.Storages.SQLServer
{
    public class ApplicationDbContext : 
        IdentityDbContext
                <AppUser,
                AppRole,
                Guid,
                AppIdentityUserClaim,
                AppIdentityUserRole,
                AppIdentityUserLogin,
                AppIdentityRoleClaim,
                AppIdentityUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new AppUserEntityTypeConfiguration());
            builder.ApplyConfiguration(new AppUserAddressEntityTypeConfiguration());
            builder.ApplyConfiguration(new AppRoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new AppIdentityRoleClaimEntityTypeConfiguration());
            builder.ApplyConfiguration(new AppIdentityUserRoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new AppIdentityUserClaimEntityTypeConfiguration());
            builder.ApplyConfiguration(new AppIdentityUserLoginEntityTypeConfiguration());
            builder.ApplyConfiguration(new AppIdentityUserTokenEntityTypeConfiguration());

        }

        public virtual DbSet<AppUserAddress> UserAddresses { get; set; }
    }
}
