using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FarmFresh.Backend.Api.Stores.Filters;
using FarmFresh.Backend.Mappers.ApiRequests2Dtos;
using FarmFresh.Backend.Mappers.Dtos2Entities;
using FarmFresh.Backend.Repositories.Implementations;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Services.Implementations.Stores;
using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using FarmFresh.Backend.Storages.SQLServer;
using GlobalExceptionHandler.WebApi;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Api.Stores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Dto2EntitiesMapperConfiguration());
                mc.AddProfile(new ApiRequests2DtosMapperConfiguration());
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
            services.AddScoped<IStoreServices, StoreServices>();
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
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalModelValidatorFilter));
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }); ; ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FarmFresh.Backend.Api.Stores", Version = "v1" });
                c.AddSecurityDefinition(GlobalConstants.StoreSwaggerSecurityDefinitionKey, new OpenApiSecurityScheme
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
                                { GlobalConstants.StoreServiceApiScope,"Store Services Api" }
                            }
                        }
                    }
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            app.UseGlobalExceptionHandler(options =>
            {
                options.ContentType = "application/json";
                options.ResponseBody((exception, httpContext) => JsonConvert.SerializeObject(new
                {
                    errorMessage = exception.Message
                }));
                options.OnError((exception, httpContext) =>
                {
                    logger.LogError(exception.ToString());
                    return Task.CompletedTask;
                });
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FarmFresh.Backend.Api.Stores v1");
                c.OAuthClientId(Configuration["IdentityServer:ClientId"]);
                c.OAuthClientSecret(Configuration["IdentityServer:ClientSecret"]);
                c.OAuthAppName("Store Services");
                c.OAuthUsePkce();

            });
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
