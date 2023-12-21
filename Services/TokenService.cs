using HubtelCommerce.DAL;
using HubtelCommerce.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HubtelCommerce.Services
{
    public static class TokenService
    {
        public static string ValidateToken(string token, JWT jwtConfig)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = authSigningkey,
                ValidIssuer = jwtConfig.ValidIssuer,
                ValidAudience = jwtConfig.ValidAudience,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true
            };

            var valid = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (valid == null) return null;

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;


            return userId;
        }

        public static async Task<string> GenerateTokenAsync(UserManager<AppUser> userManager,
                JWT jwtConfig,
                AppUser appUser)
        {

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName),
            };

            var userRoles = await userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var authSigningkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));
            var tokenOptions = new JwtSecurityToken(
                    issuer: jwtConfig.ValidIssuer,
                    audience: jwtConfig.ValidAudience,
                    expires: DateTime.Now.AddHours(jwtConfig.TokenExpiry),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningkey, SecurityAlgorithms.HmacSha512)
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
