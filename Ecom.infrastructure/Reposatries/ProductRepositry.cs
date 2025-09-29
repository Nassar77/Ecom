using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Mapster;

namespace Ecom.infrastructure.Reposatries;
public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
{
    private readonly AppDbContext _Context;
    private readonly IImageManagementService _ImageManagementService;

    public ProductRepositry(AppDbContext context,IImageManagementService imageManagementService) : base(context)
    {
        _Context = context;
        _ImageManagementService = imageManagementService;
    }

    public async Task<bool> AddAsync(AddProductDTO productDTO)
    {
        if (productDTO is null) return false;

        var product = productDTO.Adapt<Product>();

        await _Context.Products.AddAsync(product);
        await _Context.SaveChangesAsync();

        var imagePath = await _ImageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);

        var photo = imagePath.Select(path => new Photo
        {
            ImageName = path,
            ProductId=product.Id,

        }).ToList();

        await _Context.Photos.AddRangeAsync(photo);
        await _Context.SaveChangesAsync();
        return true;

    }
}
