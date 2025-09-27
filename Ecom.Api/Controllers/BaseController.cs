using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Api.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IUnitOfWork work;
   
    public BaseController(IUnitOfWork work)
    {
        this.work = work;
    }
}
