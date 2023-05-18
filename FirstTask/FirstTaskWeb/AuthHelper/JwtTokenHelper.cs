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
        private static JWTSettings _jwtSettings;
        public JwtTokenHelper(JWTSettings jWTSettings)
        {
            _jwtSettings = jWTSettings;
        }
        private enum UserRole
        {
            User,
            Admin,
        }
        public static string GenerateToken(JWTSettings jwtSettings, User model)
        {
            _jwtSettings = jwtSettings;

            /* if (jwtSetting == null)
                 return string.Empty;*/
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,model.UserId.ToString()),
                new Claim(ClaimTypes.UserData,model.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, model.FirstName + " "+ model.LastName),
                new Claim(ClaimTypes.Role, model.RoleId == 1? nameof(UserRole.User):nameof(UserRole.Admin)),
                new Claim("UserData", JsonSerializer.Serialize(model))  // Additional Claims
            };

            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Default 5 mins, max 1 day
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
            /*var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
         
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.UserData,model.UserId.ToString()),
                new Claim(ClaimTypes.Name, model.FirstName + " "+ model.LastName),
                new Claim(ClaimTypes.NameIdentifier, model.FirstName + " "+ model.LastName),
                new Claim(ClaimTypes.Role, model.RoleId == 1? nameof(UserRole.User):nameof(UserRole.Admin)),
                new Claim("UserData", JsonSerializer.Serialize(model))  // Additional Claims storing whole object
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                IssuedAt = DateTime.UtcNow,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);*/
        }
        public static bool ValidateToken(string token)
        {
            try
            {
                // Create the token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,// Validate the token's signature
                    ValidAudience = _jwtSettings.Audience,
                    ValidIssuer = _jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), // Set the secret key used to sign the token
                };

                // Create a token handler
                var tokenHandler = new JwtSecurityTokenHandler();

                // Validate the token
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                // Token is valid
                return true;
            }
            catch (SecurityTokenException)
            {
                // Token validation failed
                return false;
            }
        }
    }
}
