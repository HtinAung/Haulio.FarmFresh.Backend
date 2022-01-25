using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FarmFresh.Backend.Storages.SQLServer;
using Microsoft.EntityFrameworkCore;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Repositories.Implementations;
using FarmFresh.Backend.Entities;
using Microsoft.AspNetCore.Identity;
using FarmFresh.Backend.Shared;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FarmFresh.Backend.Crud.Tests
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public void ConfigureHost(IHostBuilder hostBuilder) =>
      hostBuilder
          .ConfigureHostConfiguration(builder => { })
          .ConfigureAppConfiguration((context, builder) => {
              builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), true, true);
              Configuration = builder.Build();
          });

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AppUser, AppRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();

            services.AddScoped<RoleManager<AppRole>>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            
            services.AddSingleton<BlobStorageHelper>((sp) =>
            {
                string blobConnectionString = Configuration.GetConnectionString("StorageAccount");
                string blobContainer = Configuration["Mode"];
                var blobContainerClient = new BlobContainerClient(blobConnectionString, blobContainer);
                blobContainerClient.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.Blob).Wait();
                return new BlobStorageHelper(blobContainerClient);
            });
            
        }
    }
}
