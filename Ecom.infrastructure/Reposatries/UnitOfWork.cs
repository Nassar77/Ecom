using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Reposatries;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _Context;

    public ICategoryRepositry CategoryRepositry { get;  }

    public IPhotoRepositry PhotoRepositry { get; }

    public IProductRepositry ProductRepositry { get; }

    public UnitOfWork(AppDbContext context)
    {
        _Context = context;
        CategoryRepositry=new CategoryRepositry(_Context);
        PhotoRepositry = new PhotoRepositry(_Context);
        ProductRepositry = new ProductRepositry(_Context);
    }
}
