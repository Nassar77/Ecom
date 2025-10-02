using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Api.Controllers;

public class BugController : BaseController
{
    public BugController(IUnitOfWork work) : base(work)
    {
    }
    [HttpGet("not-found")]
    public async Task<ActionResult> GetNotFound()
    {
        var category = await work.CategoryRepositry.GetByIDAsync(100);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpGet("server-error")]
    public async Task<ActionResult> GetServerError()
    {
        var category = await work.CategoryRepositry.GetByIDAsync(100);
        category.Name = ""; // 
        return Ok(category);
    }

    [HttpGet("bad-request/{id}")]
    public async Task<ActionResult> GetBadRequest(int id)
    {
        return Ok();
    }

    [HttpGet("bad-request")]
    public async Task<ActionResult> GetBadRequest()
    {
        return BadRequest();
    }
}
