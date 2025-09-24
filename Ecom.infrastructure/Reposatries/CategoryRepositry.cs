using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Reposatries;
public class CategoryRepositry : GenericRepositry<Category>, ICategoryRepositry
{
    public CategoryRepositry(AppDbContext context) : base(context)
    {
    }
}
