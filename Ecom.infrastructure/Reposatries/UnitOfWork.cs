using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Reposatries;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _Context;
    private readonly IImageManagementService _ImageManagementService;

    public ICategoryRepositry CategoryRepositry { get;  }

    public IPhotoRepositry PhotoRepositry { get; }

    public IProductRepositry ProductRepositry { get; }

    public UnitOfWork(AppDbContext context,IImageManagementService imageManagementService)
    {
        _Context = context;
        _ImageManagementService = imageManagementService;
        CategoryRepositry =new CategoryRepositry(_Context);
        PhotoRepositry = new PhotoRepositry(_Context);
        ProductRepositry = new ProductRepositry(_Context, _ImageManagementService);
    }
}
