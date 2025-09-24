namespace Ecom.Core.Interfaces;
public interface IUnitOfWork
{
    public ICategoryRepositry  CategoryRepositry { get; }
    public IPhotoRepositry  PhotoRepositry { get; }
    public IProductRepositry  ProductRepositry { get; }
}
