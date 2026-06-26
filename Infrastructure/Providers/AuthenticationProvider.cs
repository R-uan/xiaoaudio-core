using AudioArchive.Database.Entity;
using AudioArchive.Infrastructure.Settings;

using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AudioArchive.Infrastructure.Providers {
  public class AuthenticationProvider : IAuthenticationProvider
  {
    private readonly JwtSettings _jwt;

    public AuthenticationProvider(IOptions<JwtSettings> jwtOptions)
    {
      _jwt = jwtOptions.Value;
    }

    public string GenerateToken(Account subject)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Email, subject.Email),
        new Claim(JwtRegisteredClaimNames.Sub, subject.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      };

      var token = new JwtSecurityToken(
        issuer: _jwt.Issuer,
        audience: _jwt.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}