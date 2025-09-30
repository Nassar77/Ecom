using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

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
    public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
    {
        if(updateProductDto is null) return false;

        var findProduct= await _Context.Products.Include(m=>m.Category)
            .Include(m=>m.Photos)
            .FirstOrDefaultAsync(m=>m.Id==updateProductDto.Id);

        if (findProduct is null) return false;

        findProduct = updateProductDto.Adapt(findProduct);

        var findPhoto =await _Context.Photos.Where(m=>m.ProductId==updateProductDto.Id).ToListAsync();

        foreach (var item in findPhoto)
        {
            _ImageManagementService.DeleteImageAsync(item.ImageName);
        }
        _Context.Photos.RemoveRange(findPhoto);

        var imagePath = await _ImageManagementService.AddImageAsync(updateProductDto.Photo, updateProductDto.Name);

        var photo = imagePath.Select(path => new Photo
        {
            ImageName = path,
            ProductId = updateProductDto.Id,

        }).ToList();

        await _Context.Photos.AddRangeAsync(photo);
        await _Context.SaveChangesAsync();

        return true;
    }
    public async Task DeleteAsync(Product product)
    {
        var photos=await _Context.Photos.Where(x=>x.ProductId==product.Id).ToListAsync();
        foreach (var item in photos)
        {
            _ImageManagementService.DeleteImageAsync(item.ImageName);
        }
         _Context.Products.Remove(product);
        await _Context.SaveChangesAsync();
    }

}
