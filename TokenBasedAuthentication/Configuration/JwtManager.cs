using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TokenBasedAuthentication.Data.Entity;

namespace TokenBasedAuthentication.Configuration;

public class JwtManager
{
    private readonly JwtConfig _config;

    public JwtManager(IOptionsMonitor<JwtConfig> monitor)
    {
        _config = monitor.CurrentValue;
    }

    public string GenerateToken(AppUser user)
    {
        var key = Encoding.ASCII.GetBytes(_config.Secret);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsIdentity(new Claim[]
        {
            new Claim("Id", user.Id),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        });
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claims,
            Expires = DateTime.Now.AddHours(72),
            SigningCredentials = credentials,
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return token;
    }

    public AppUser Authenticate(ClaimsIdentity identity)
    {
        var userClaims = identity.Claims;
        var user = new AppUser()
        {
            Id = userClaims.FirstOrDefault(i => i.Type == "Id")?.Value,
            UserName = userClaims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value,
            Email = userClaims.FirstOrDefault(i => i.Type == ClaimTypes.Email)?.Value,
        };
        return user;
    }
}