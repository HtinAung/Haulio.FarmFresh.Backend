// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Storages.SQLServer;

namespace FarmFresh.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();


                    var customerUser = userMgr.FindByNameAsync("ghulamcyber@hotmail.com").Result;
                    if (customerUser == null)
                    {
                        customerUser = new AppUser
                        {
                            UserName = "ghulamcyber@hotmail.com",
                            Email = "ghulamcyber@hotmail.com",
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                            FullName = "Mirza Ghulam Rasyid"
                        };
                        var result = userMgr.CreateAsync(customerUser, "@Future30").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(customerUser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, customerUser.FullName),
                            new Claim(JwtClaimTypes.Email, customerUser.Email),
                            new Claim(JwtClaimTypes.Role, GlobalConstants.CustomerUserRoleName)
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug($"{customerUser.FullName} ({customerUser.Email}) created successfully");
                    }
                    else
                    {
                        Log.Debug($"{customerUser.FullName} ({customerUser.Email}) already exists");
                    }

                }
            }
        }
    }
}
