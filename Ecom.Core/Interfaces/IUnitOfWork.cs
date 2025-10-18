namespace Ecom.Core.Interfaces;
public interface IUnitOfWork
{
    public ICategoryRepositry  CategoryRepositry { get; }
    public IPhotoRepositry  PhotoRepositry { get; }
    public IProductRepositry  ProductRepositry { get; }
    public ICustomerBasketRepositry CustomerBasket { get; }
    public IAuth Auth { get; }
}
