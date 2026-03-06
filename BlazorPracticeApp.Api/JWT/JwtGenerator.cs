using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorPracticeApp.Api.JWT
{
    public class JwtGenerator
    {

        private readonly string secretKey;
        public JwtGenerator(IConfiguration configuration)
        {
            secretKey = configuration["Jwt:Key"] ?? throw new Exception("Jwt не найден");
        }

        public string GenerateJwt(int userId, int? roleId)
        {
            var claims = new[]
            {
            new Claim("userId", userId.ToString()),
            new Claim("roleId", roleId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddHours(2)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
