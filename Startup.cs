using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using MusicDb.Models;
using MusicDb.Services;

namespace MusicDb {

  public class Startup {

    public IConfiguration Configuration { get; private set; }

    // Connects appsettings.json config file as an object
    public Startup(IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services) {
      services.AddMvc();
      services.AddSession();

      // Configure Entity Framework to use MySql
      Action<DbContextOptionsBuilder> useMySqlAppSettings =
        options => options.UseMySql(Configuration["DbConfig:ConnectionString"]);
      services.AddDbContext<Context>(useMySqlAppSettings);

      // Configure API proxier service with keys
      services.AddSingleton<IConfiguration>(Configuration.GetSection("ApiConfig"));
      services.AddScoped<ApiService>();
      services.AddScoped<ArtistService>();
    }

    public void Configure(
      IApplicationBuilder app,
      IHostingEnvironment env,
      ILoggerFactory loggerFactory
    ) {
      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
      loggerFactory.AddConsole();
      app.UseStaticFiles();
      app.UseSession();
      app.UseMvc();
    }

  }
}
