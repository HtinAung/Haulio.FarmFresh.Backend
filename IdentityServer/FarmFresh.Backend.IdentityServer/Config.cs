// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace FarmFresh.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                        new IdentityResources.Email(),
                        new IdentityResource(GlobalConstants.RoleIdentityResource, new string[]
                        {
                            GlobalConstants.RoleIdentityResource
                        })
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(GlobalConstants.CustomerServiceApiScope),
                new ApiScope(GlobalConstants.StoreServiceApiScope)
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource(GlobalConstants.ApiAudience, "FarmFresh Api")
                {
                    Scopes =
                    {
                        GlobalConstants.CustomerServiceApiScope,
                        GlobalConstants.StoreServiceApiScope
                    },
                    UserClaims =
                    {
                        "name",
                        "email",
                        "role"
                    }
                }
            };

        public static IEnumerable<Client> Clients(IConfiguration configuration) =>
            new Client[]
            {
                // swagger client
                new Client
                {
                    ClientId = configuration["ClientId"],
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret(configuration["ClientSecret"].Sha256()) },

                    AllowedScopes = 
                    {
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        OidcConstants.StandardScopes.Email,
                        OidcConstants.StandardScopes.OfflineAccess,
                        GlobalConstants.RoleIdentityResource,
                        GlobalConstants.CustomerServiceApiScope,
                        GlobalConstants.StoreServiceApiScope
                    },
                    AllowOfflineAccess = true,
                    RedirectUris =
                    {
                        $"{configuration["CustomerApiAddress"]}/swagger/oauth2-redirect.html",
                        $"{configuration["StoreApiAddress"]}/swagger/oauth2-redirect.html",
                    },
                    AllowedCorsOrigins =
                    {
                        configuration["CustomerApiAddress"],
                        configuration["StoreApiAddress"]
                    }
                },

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},
            };
    }
}