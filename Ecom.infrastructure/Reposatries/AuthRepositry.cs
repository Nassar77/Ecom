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
    private readonly IGenerateToken _GenerateToken;

    public AuthRepositry(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IGenerateToken generateToken)
    {
        _UserManager = userManager;
        _SignInManager = signInManager;
        _EmailService = emailService;
        _GenerateToken = generateToken;
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
        //generate token
        string token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
        //send active email
        await SendEmail(user.Email, token, "Active", "Active Email", "Please active your email,Clicke the bytton");
        return "Done";
    }

    private async Task SendEmail(string email, string token, string component, string subject, string message)
    {
        var result = new EmailDTO(email,
            "austin31@ethereal.email", subject,
            EmailStringBody.send(email, token, component, message));
        await _EmailService.SendEmail(result);
    }

    public async Task<string> LoginAsync(LoginDto login)
    {
        if (login == null)
        {
            return null;
        }
        var findUser = await _UserManager.FindByEmailAsync(login.Email);
        if (findUser == null)
            return "the Email not found";
        if (!findUser.EmailConfirmed)
        {
            string token=await _UserManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "Active", "Active Email", "Please active your email,Clicke the bytton");
            return "Please confirm your email first, we have send active to your E-mail";
        }

        var result=await _SignInManager.CheckPasswordSignInAsync(findUser, login.Password,true);
        if(result.Succeeded)
        {
            return _GenerateToken.GetAndCreateToken(findUser);
        }
        return "Please check your email and password, something went wrong";

    }

    public async Task<bool> SendEmailForForgetPassword(string email)
    {
        var findUser = await _UserManager.FindByEmailAsync(email);
        if (findUser is null)
        {
            return false;
        }

        var token = await _UserManager.GeneratePasswordResetTokenAsync(findUser);

        await SendEmail(
            email: findUser.Email,
            token: token,
            component: "Reset-Password",
            subject: "Rest password",
            message: "Click on button to Reset your password" 
        );

        return true;
    }

    public async Task<string> ResetPassword(ResetPasswordDto restPassword)
    {
        var findUser = await _UserManager.FindByEmailAsync(email: restPassword.Email);

        if (findUser is null)
        {
            return null;
        }

        var  result = await _UserManager.ResetPasswordAsync(
            user: findUser,
            token: restPassword.Token,
            newPassword: restPassword.Password  
        );

        if (result.Succeeded)
        {
            return "Password change success";
        }

        return result.Errors.ToList()[index: 0].Description;
    }

    public async Task<bool>ActiveAccount(ActiveAccountDto accountDTO)
    {
        var findUser = await _UserManager.FindByEmailAsync(accountDTO.Email);
        if (findUser == null)
        {
            return false;
        }

        var reslt = await _UserManager.ConfirmEmailAsync(findUser, accountDTO.Token);
        if (reslt.Succeeded)
            return true;

        var token = await _UserManager.GenerateEmailConfirmationTokenAsync(findUser);
        await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please active your email, click the link");

        return false;

    }
}
