using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Reposatries;
public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
{
    public ProductRepositry(AppDbContext context) : base(context)
    {
    }
}
