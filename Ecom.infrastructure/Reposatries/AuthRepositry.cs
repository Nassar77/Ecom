using Ecom.Core.DTO;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Identity;

namespace Ecom.infrastructure.Reposatries;
public class AuthRepositry : IAuth
{
    private readonly UserManager<AppUser> _UserManager;
    private readonly IEmailService _EmailService;
    private readonly SignInManager<AppUser> _SignInManager;

    public AuthRepositry(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
    {
        _UserManager = userManager;
        _SignInManager = signInManager;
        _EmailService = emailService;
    }
    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        if (registerDto == null)
        {
            return null;
        }
        if (await _UserManager.FindByNameAsync(registerDto.UserName) is not null)
        {
            return "the user name is already register";
        }
        if (await _UserManager.FindByEmailAsync(registerDto.Email) is not null)
        {
            return "the Email is already register";
        }
        AppUser user = new AppUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
        };
        var result = await _UserManager.CreateAsync(user);
        if (result.Succeeded is not true)
        {
            return result.Errors.ToList()[0].Description;
        }
        string code=await _UserManager.GenerateEmailConfirmationTokenAsync(user);
        //send active email
        await SendEmail(user.Email,code, "Active", "Active Email", "Please active your email,Clicke the bytton");
        return "Done";
    }

    private async Task SendEmail(string email, string code, string component, string subject, string message)
    {
        var result = new EmailDTO(email,
            "austin31@ethereal.email", subject,
            EmailStringBody.send(email, code, component, message));
        await _EmailService.SendEmail(result);
    }


}
