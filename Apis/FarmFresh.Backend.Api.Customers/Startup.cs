using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FarmFresh.Backend.Api.Customers.Filters;
using FarmFresh.Backend.Mappers.Dtos2Entities;
using FarmFresh.Backend.Repositories.Implementations;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Services.Implementations.Customers;
using FarmFresh.Backend.Services.Implementations.Stores;
using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using FarmFresh.Backend.Storages.SQLServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Api.Customers
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

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Dto2EntitiesMapperConfiguration());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<ICustomerServices, CustomerServices>();
            services.AddSingleton<BlobStorageHelper>((sp) =>
            {
                string blobConnectionString = Configuration.GetConnectionString("StorageAccount");
                string blobContainer = Configuration["Mode"];
                var blobContainerClient = new BlobContainerClient(blobConnectionString, blobContainer);
                blobContainerClient.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.Blob).Wait();
                return new BlobStorageHelper(blobContainerClient);
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = Configuration["IdentityServer:BaseAddress"];
                    options.Audience = Configuration["IdentityServer:Audience"];
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FarmFresh.Backend.Api.Customers", Version = "v1" });
                c.AddSecurityDefinition(GlobalConstants.CustomerSwaggerSecurityDefinitionKey, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Configuration["IdentityServer:BaseAddress"]}/connect/authorize"),
                            TokenUrl = new Uri($"{Configuration["IdentityServer:BaseAddress"]}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { GlobalConstants.CustomerServiceApiScope,"Customer Services Api" }
                            }
                        }
                    }
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FarmFresh.Backend.Api.Customers v1");
                    c.OAuthClientId(Configuration["IdentityServer:ClientId"]);
                    c.OAuthClientSecret(Configuration["IdentityServer:ClientSecret"]);
                    c.OAuthAppName("Customer Services");
                    c.OAuthUsePkce();

                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
