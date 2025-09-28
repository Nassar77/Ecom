using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Mapster;

namespace Ecom_Api.Mapping;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductDTO, Product>()
            .Map(dest => dest.Category.Name, src => src.CategoryName);
        //throw new NotImplementedException();
    }
}
