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

namespace FarmFresh.Backend.Repositories.Tests
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
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        }
    }
}
