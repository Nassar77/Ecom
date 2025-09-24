using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Api.Controllers;

public class CategoriesController(IUnitOfWork work) : BaseController(work)
{
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories=await _work.CategoryRepositry.GetAllAsync();
            if (categories is null)
                return BadRequest();
            return Ok(categories);

        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

}
