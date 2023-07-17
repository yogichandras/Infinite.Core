using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace INFINITE.CORE.MVC.Authorization
{
    public class AuthHelper
    {
        public IConfiguration Configuration { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public CoreSession Session { get 
            {
                if (Configuration != null && HttpContextAccessor != null)
                {
                    var token = HttpContextAccessor.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == Configuration["ApplicationConfig:Issuer"]);
                    if (!string.IsNullOrEmpty(token.Value))
                    {
                        return GetSession(token.Value);
                    }
                }
                return default;     
            } 
        }
        
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
        public CoreSession GetSession(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(token);
            var permissions = new List<string>();
            var claimPermission = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
            if (claimPermission != null && !string.IsNullOrEmpty(claimPermission.Value))
            {
                permissions = JsonConvert.DeserializeObject<List<string>>(claimPermission.Value);
            }

            var session = new CoreSession
            {
                Id = claims.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value,
                Email = claims.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value,
                Username = claims.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value,
                Permissions = permissions
            };
            return session;
        }
    }
}
