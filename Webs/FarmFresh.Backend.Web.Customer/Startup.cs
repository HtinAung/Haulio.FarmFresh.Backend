using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace FarmFresh.Backend.Web.Customer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                           .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                           {
                               options.LoginPath = new PathString("/auth/login");
                               options.LogoutPath = new PathString("/auth/logout");
                               options.AccessDeniedPath = new PathString("/auth/accessdenied");
                           })
                           .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                           {
                               options.Authority = Configuration["IdentityServer:BaseAddress"];
                               options.ClientId = Configuration["IdentityServer:ClientId"];
                               options.ClientSecret = Configuration["IdentityServer:ClientSecret"];
                               options.ResponseType = OidcConstants.ResponseTypes.Code;
                               options.SaveTokens = true;
                               options.GetClaimsFromUserInfoEndpoint = true;
                               options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                               options.TokenValidationParameters.NameClaimType = "name";
                               options.TokenValidationParameters.RoleClaimType = "role";

                               foreach (string scope in Configuration["IdentityServer:Scopes"].Split(" ", StringSplitOptions.RemoveEmptyEntries))
                               {
                                   options.Scope.Add(scope);
                               }
                               options.CallbackPath = "/signin-oidc";
                               options.SignedOutCallbackPath = "/signout-callback-oidc";
                               options.ClaimActions.MapAll();
                           });


            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();
            services.AddUserAccessTokenClient("Api", options =>
            {
                options.BaseAddress = new Uri(Configuration["ResourceEndpoints:BaseAddress"]);
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
