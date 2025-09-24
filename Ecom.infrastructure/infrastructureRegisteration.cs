using Ecom.Core.Interfaces;
using Ecom.infrastructure.Reposatries;
using Microsoft.Extensions.DependencyInjection;


namespace Ecom.infrastructure;
public static class infrastructureRegisteration
{
    public static IServiceCollection infrastructureConfiguration(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

        //services.AddScoped<ICategoryRepositry, CategoryRepositry>();
        //services.AddScoped<IPhotoRepositry, PhotoRepositry>();
        //services.AddScoped<IProductRepositry, ProductRepositry>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
