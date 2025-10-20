namespace Ecom.Core.DTO;
public record RegisterDto:LoginDto
{
    public string UserName { get; set; }
   

}
public record LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; } 
}
public record ResetPasswordDto:LoginDto
{
    public string Token { get; set; }
}


public record ActiveAccountDto
{
    public string Email { get; set; }
    public string Token { get; set; }
}