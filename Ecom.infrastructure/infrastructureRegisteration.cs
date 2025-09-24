using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Reposatries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Ecom.infrastructure;
public static class infrastructureRegisteration
{
    public static IServiceCollection infrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

        //services.AddScoped<ICategoryRepositry, CategoryRepositry>();
        //services.AddScoped<IPhotoRepositry, PhotoRepositry>();
        //services.AddScoped<IProductRepositry, ProductRepositry>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
        });
        return services;
    }
}
