using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Ecom.infrastructure.Reposatries;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _Context;
    private readonly IImageManagementService _ImageManagementService;
    private readonly IConnectionMultiplexer _Redis;
    private readonly UserManager<AppUser> _UserManager;
    private readonly IEmailService _EmailService;
    private readonly SignInManager<AppUser> _SignINManager;
    private readonly IGenerateToken _Token;

    public ICategoryRepositry CategoryRepositry { get;  }

    public IPhotoRepositry PhotoRepositry { get; }

    public IProductRepositry ProductRepositry { get; }

    public ICustomerBasketRepositry CustomerBasket {  get; }

    public IAuth Auth { get; }

    public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signINManager, IGenerateToken token)
    {
        _Context = context;
        _ImageManagementService = imageManagementService;
        _Redis = redis;
        _UserManager = userManager;
        _EmailService = emailService;
        _SignINManager = signINManager;
        _Token = token;
        CategoryRepositry = new CategoryRepositry(_Context);
        PhotoRepositry = new PhotoRepositry(_Context);
        ProductRepositry = new ProductRepositry(_Context, _ImageManagementService);
        CustomerBasket = new CustomerBasketRepositry(_Redis);
        Auth = new AuthRepositry(userManager, signINManager, emailService,token);
    }
}
