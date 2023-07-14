using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace INFINITE.CORE.MVC.Authorization
{
    public class AuthHelper
    {
        public IConfiguration Configuration { get; set; }
        public SecurityToken ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return default;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["ApplicationConfig:Issuer"],
                    ValidAudience = Configuration["ApplicationConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["ApplicationConfig:SecretKey"]))
                }, out SecurityToken validatedToken);
                return validatedToken;
            }
            catch
            {
                return default;
            }
        }
    }
}
