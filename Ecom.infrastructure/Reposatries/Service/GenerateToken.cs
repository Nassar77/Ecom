using Ecom.Core.Entities;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecom.infrastructure.Reposatries.Service;
public class GenerateToken : IGenerateToken
{
    private readonly IConfiguration _configuration;

    public GenerateToken(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public  string GetAndCreateToken(AppUser user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(type: ClaimTypes.Name, value: user.UserName),
            new Claim(type: ClaimTypes.Email, value: user.Email),
        };

        string Security = _configuration["Token:Secret"];
        var key = Encoding.ASCII.GetBytes(s: Security);

        SigningCredentials credentials = new SigningCredentials(key: new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(value: 1),
            Issuer = _configuration["Token:Issure"],
            SigningCredentials = credentials,
            NotBefore = DateTime.Now,
        };

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token: securityToken);
    }
}
