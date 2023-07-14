using Microsoft.AspNetCore.Mvc;
using INFINITE.CORE.Shared.Attributes;
using MediatR;
using INFINITE.CORE.Shared.Interface;
using AutoMapper;
using System.Security.Claims;

namespace INFINITE.CORE.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController<T> : Controller
    {
        private IMediator _mediatorInstance;
        private ILogger<T> _loggerInstance;
        private IWrapperHelper _wrapperInstance;
        private IMapper _mapperInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected IWrapperHelper _wrapper => _wrapperInstance ??= HttpContext.RequestServices.GetService<IWrapperHelper>();
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
        protected IActionResult Wrapper<TT>(TT val)
        {
            dynamic result = val!;
            int code = result.Code;
            return this.StatusCode(code, val);
        }

        protected TokenObject Token
        {
            get
            {
                var result = new TokenObject();
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var requestKey))
                {
                    if (requestKey.Count > 0)
                    {
                        var key = requestKey.First().Split(' ');
                        if (key.Length == 2 && key[0].ToLower().Trim() == "bearer")
                        {
                            var identity = HttpContext.User.Identity as ClaimsIdentity;
                            if (identity != null && identity.Claims != null && identity.Claims.Count() > 0)
                            {
                                var token_exp = identity.Claims.FirstOrDefault(claim => claim.Type.Equals("exp")).Value;
                                var ticks = long.Parse(token_exp);
                                result.RawToken = key[1];
                                result.RefreshToken = identity.Claims.FirstOrDefault(x => x.Type == "token")?.Value;
                                result.ExpiredAt = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;

                                var claimRole = identity.Claims.FirstOrDefault(d => d.Type == ClaimTypes.Role);

                                string roles = claimRole != null ? claimRole.Value : string.Empty;
                                result.User = new TokenUserObject()
                                {
                                    FullName = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
                                    Id = Guid.Parse(identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value),
                                    Mail = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                                    Username = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                                    Role = new List<ReferensiStringObject>()
                                };
                                if (!string.IsNullOrEmpty(roles))
                                {
                                    var roles_string = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(roles);
                                    result.User.Role = roles_string.Select(d => new ReferensiStringObject()
                                    {
                                        Id = d.Split('-')[0],
                                        Nama = d.Split('-')[1],
                                    }).ToList();
                                }
                            }
                        }
                    }
                }
                return result;
            }
        }
        protected string Inputer
        {
            get
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                string id = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                string name = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                return $"{id}|{name}";
            }
        }
    }

}
