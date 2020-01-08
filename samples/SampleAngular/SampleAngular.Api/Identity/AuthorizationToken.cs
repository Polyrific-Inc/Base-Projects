using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleAngular.Api.Identity
{
    public class AuthorizationToken
    {
        public static string GenerateToken(int userId, string userName, string firstName, string lastName, 
            string userRole, string tokenKey, string tokenIssuer, string tokenAudience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var token = new JwtSecurityToken(
                issuer: tokenIssuer,
                audience: tokenAudience,
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.GivenName, firstName ?? string.Empty),
                    new Claim(ClaimTypes.Surname, lastName ?? string.Empty),
                    new Claim(ClaimTypes.Role, userRole)
                },
                expires: DateTime.UtcNow.AddHours(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
