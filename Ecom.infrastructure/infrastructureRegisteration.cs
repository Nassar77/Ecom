using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Reposatries;
using Ecom.infrastructure.Reposatries.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;


namespace Ecom.infrastructure;
public static class infrastructureRegisteration
{
    public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

        //Apply unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //for register email sender
        services.AddScoped<IEmailService, EmailService>();
        //for imageservice
        services.AddScoped<IImageManagementService, ImageManagementService>();
        services.AddSingleton<IFileProvider>(
          new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
          );
        //apply redis connection
        services.AddSingleton<IConnectionMultiplexer>(i =>
        {
            var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
            return ConnectionMultiplexer.Connect(config);
        });


        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
        });

        return services;
    }
}
