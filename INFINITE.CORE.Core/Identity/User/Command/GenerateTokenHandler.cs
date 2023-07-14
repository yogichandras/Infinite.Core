using AutoMapper;
using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Shared.Attributes;
using Microsoft.Extensions.Options;
using INFINITE.CORE.Core.Helper;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace INFINITE.CORE.Core.User.Command
{

    #region Request
    public class GenerateTokenMapping : Profile
    {
        public GenerateTokenMapping()
        {
            CreateMap<GenerateTokenRequest, TokenUserObject>().ReverseMap();
        }
    }
    internal class GenerateTokenRequest : TokenUserObject, IRequest<ObjectResponse<TokenObject>>
    {
        public string RefreshToken { get; set; }
        public DateTime? Expired { get; set; }
    }
    #endregion

    internal class GenerateTokenHandler : IRequestHandler<GenerateTokenRequest, ObjectResponse<TokenObject>>
    {
        private readonly ILogger _logger;
        private readonly ApplicationConfig _config;
        public GenerateTokenHandler(
            ILogger<GenerateTokenHandler> logger,
            IOptions<ApplicationConfig> config
            )
        {
            _logger = logger;
            _config = config.Value;
        }
#pragma warning disable CS1998 // If you have permission table search in here 
        public async Task<ObjectResponse<TokenObject>> Handle(GenerateTokenRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 
        {
            ObjectResponse<TokenObject> result = new ObjectResponse<TokenObject>();
            try
            {
                var refresh_token = request.RefreshToken;
                var token_handler = new JwtSecurityTokenHandler();
                var claims = new List<Claim> {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, request.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                            new Claim(JwtRegisteredClaimNames.GivenName, request.FullName),
                            new Claim(JwtRegisteredClaimNames.Email, request.Mail),
                            new Claim("token" , refresh_token),
                            };

                if (request.Role != null && request.Role.Count > 0)
                {
                    List<string> claim_roles = request.Role.Select(d => $"{d.Id}-{d.Nama}").ToList();
                    claims.Add(new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(claim_roles))); ;
                }

                var key = Encoding.ASCII.GetBytes(_config.SecretKey);
                DateTime expired = request.Expired.HasValue ? request.Expired.Value : DateTime.Now.AddMinutes(_config.TokenExpired);
                var token = new JwtSecurityToken(issuer: _config.Issuer,
                                audience: _config.Audience,
                                claims: claims,
                                expires: expired,
                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                            );

                result.Data = new TokenObject()
                {
                    ExpiredAt = expired,
                    RefreshToken = refresh_token,
                    RawToken = new JwtSecurityTokenHandler().WriteToken(token),
                    User = request
                };
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Generate Token", request);
                result.Error("Failed Generate Token", ex.Message);
            }
            return result;
        }
    }
}

