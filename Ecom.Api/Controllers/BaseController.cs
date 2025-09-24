using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Api.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class BaseController(IUnitOfWork work) : ControllerBase
{
    protected readonly IUnitOfWork _work = work;
}
