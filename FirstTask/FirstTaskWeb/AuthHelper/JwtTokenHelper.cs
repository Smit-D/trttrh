using FirstTask.Entities.Models;
using FirstTaskWeb.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FirstTaskWeb.AuthHelper
{
    public class JwtTokenHelper
    {
        private enum AdminRole
        {
            User,
            Admin,
        }
        public static string GenerateToken(JWTSettings jwtSetting, User model)
        {

            if (jwtSetting == null)
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.UserData,model.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, model.FirstName + " "+ model.LastName),
                new Claim(ClaimTypes.Role, model.RoleId == 1? nameof(AdminRole.User):nameof(AdminRole.Admin)),
                new Claim("UserData", JsonSerializer.Serialize(model))  // Additional Claims
            };

            var token = new JwtSecurityToken(
                jwtSetting.Issuer,
                jwtSetting.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Default 5 mins, max 1 day
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
