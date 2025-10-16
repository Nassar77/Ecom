using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using StackExchange.Redis;

namespace Ecom.infrastructure.Reposatries;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _Context;
    private readonly IImageManagementService _ImageManagementService;
    private readonly IConnectionMultiplexer _Redis;

    public ICategoryRepositry CategoryRepositry { get;  }

    public IPhotoRepositry PhotoRepositry { get; }

    public IProductRepositry ProductRepositry { get; }

    public ICustomerBasketRepositry CustomerBasket {  get; }

    public UnitOfWork(AppDbContext context,IImageManagementService imageManagementService,IConnectionMultiplexer redis)
    {
        _Context = context;
        _ImageManagementService = imageManagementService;
        _Redis = redis;
        CategoryRepositry =new CategoryRepositry(_Context);
        PhotoRepositry = new PhotoRepositry(_Context);
        ProductRepositry = new ProductRepositry(_Context, _ImageManagementService);
        CustomerBasket = new CustomerBasketRepositry(_Redis);
    }
}
