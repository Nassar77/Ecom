using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Ecom_Api.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Api.Controllers;
public class ProductsController : BaseController
{
    public ProductsController(IUnitOfWork work) : base(work)
    {
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await work.ProductRepositry
                .GetAllAsync(x => x.Category, x => x.Photos);

            if (products is null)
                return BadRequest(new ResponseAPI(400));

            var result = products.Adapt<List<ProductDTO>>();

            return Ok(result);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await work.ProductRepositry
                .GetByIDAsync(id, x => x.Category, x => x.Photos);

            if (product is null)
                return BadRequest(new ResponseAPI(400, $"not found product id={id}"));

            var result = product.Adapt<ProductDTO>();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseAPI(400, ex.Message));
        }
    }
    [HttpPost("")]
   public async Task<IActionResult>Add(AddProductDTO productDto)
    {
        try
        {
           // var product=productDto.Adapt<Product>();
            await work.ProductRepositry.AddAsync(productDto);
            return Ok(new ResponseAPI(200,"Product is added"));
        }
        catch (Exception ex)
        {

            return BadRequest(new ResponseAPI(400, ex.Message));
        }
    }
}
