using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom_Api.Helper;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ecom_Api.Controllers;

public class CategoriesController : BaseController
{
    public CategoriesController(IUnitOfWork work) : base(work)
    {
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories = await work.CategoryRepositry.GetAllAsync();
            if (categories is null)
                return BadRequest(new ResponseAPI(400));
            return Ok(categories);

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
            var entity = await work.CategoryRepositry.GetByIDAsync(id);
            if (entity is null)
                return BadRequest(new ResponseAPI(400,$"not found category id={id}"));
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("")]
    public async Task<IActionResult> Add(CategoryDto categoryDTO)
    {
        try
        {
            var category = categoryDTO.Adapt<Category>();
            await work.CategoryRepositry.AddAsync(category);
            return Ok(new ResponseAPI(200,"Item has been added." ));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("")]
    public async Task<IActionResult> Update(UpdateCategoryDto categoryDTO)
    {
        try
        {
            var category = categoryDTO.Adapt<Category>();
            await work.CategoryRepositry.UpdateAsync(category);
            return Ok(new ResponseAPI(200, "Item has been updated"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await work.CategoryRepositry.DeleteAsync(id);
            return Ok(new ResponseAPI(200,"Item has been deleted" ));
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

}
