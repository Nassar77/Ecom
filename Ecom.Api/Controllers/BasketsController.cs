using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom_Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Api.Controllers;

public class BasketsController : BaseController
{
    public BasketsController(IUnitOfWork work) : base(work)
    {
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await work.CustomerBasket.GetBasketAsync(id);
        if(result is null) 
          return  Ok(new CustomerBasket());

        return Ok(result);
    }
    [HttpPost("")]
    public async Task<IActionResult> Add(CustomerBasket basket)
    {
        var _basket = await work.CustomerBasket.UpdateBasketAsync(basket);
        return Ok(basket);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await work.CustomerBasket.DeleteBasketAsync(id);
        return result ? Ok(new ResponseAPI(200, "Item is deleted")) : BadRequest(new ResponseAPI(400));
    }

}
