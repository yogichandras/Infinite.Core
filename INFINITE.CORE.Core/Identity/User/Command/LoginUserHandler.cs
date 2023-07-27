using AutoMapper;
using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Core.Helper;
using INFINITE.CORE.Shared.Interface;
using Microsoft.Extensions.Options;
using INFINITE.CORE.Core.User.Command;

namespace INFINITE.CORE.Core.User.Command
{

    #region Request
    public class LoginRequest : IRequest<ObjectResponse<TokenObject>>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    #endregion

    internal class LoginUserHandler : IRequestHandler<LoginRequest, ObjectResponse<TokenObject>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IWrapperHelper _wrapper;
        private readonly IGeneralHelper _helper;
        private readonly ApplicationConfig _config;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public LoginUserHandler(
            ILogger<LoginUserHandler> logger,
            IMediator mediator,
            IWrapperHelper wrapper,
            IGeneralHelper helper,
            IOptions<ApplicationConfig> config,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _wrapper = wrapper;
            _helper = helper;
            _config = config.Value;
            _context = context;
        }
        public async Task<ObjectResponse<TokenObject>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<TokenObject> result = new ObjectResponse<TokenObject>();
            try
            {
                string _hash = _helper.PasswordEncrypt(request.Password);
                var user = await _context.Entity<Data.Model.User>()
                            .Where(d => d.Username.ToLower() == request.Username.ToLower())
                            .Include(d => d.UserRole).ThenInclude(d => d.IdRoleNavigation)
                            .ThenInclude(d => d.RolePermissions)
                            .OrderByDescending(d=>d.CreateDate).FirstOrDefaultAsync();

                if (user != null)
                {
                    if (user.IsLockout)
                    {
                        result.Forbidden($"User has been Locked! call administrator for unlock!");
                        return result;
                    }
                    if (user.Password != _hash)
                    {
                        user.AccessFailedCount++;
                        if (user.AccessFailedCount > _config.MaximumLoginRetry)
                        {
                            user.IsLockout = true;
                            user.UpdateBy = $"{user.Id}|{user.Fullname}";
                            user.UpdateDate = DateTime.Now;
                            _wrapper.Response(ref result, _wrapper.Response(await _context.UpdateSave(user)));
                            result.Forbidden($"User has been Locked call administrator for unlock!");
                        }
                        else
                        {
                            _wrapper.Response(ref result, _wrapper.Response(await _context.UpdateSave(user)));
                            result.Forbidden($"Failed login please check again your password!{Environment.NewLine} Retry {user.AccessFailedCount} of {_config.MaximumLoginRetry}");

                        }
                        return result;
                    }

                    user.AccessFailedCount = 0;
                    user.IsLockout = false;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateBy = $"{user.Id}|{user.Fullname}";
                   
                    var generateToken = await _mediator.Send(new GenerateTokenRequest()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = user.Fullname,
                        Mail = user.Mail,
                        Role = user.UserRole.Select(d=>new ReferensiStringObject()
                        {
                            Id = d.IdRole,
                            Nama = d.IdRoleNavigation.Name
                        }).ToList(),
                        Permissions = user.UserRole.Select(d=> d.IdRoleNavigation.RolePermissions.Select(c => c.Permission)).SelectMany(c => c).GroupBy(c => c).Select(c => c.Key).ToList(),
                        Expired = null,
                        RefreshToken = Guid.NewGuid().ToString()
                    });
                    if (generateToken.Succeeded)
                    {
                        result.Data = generateToken.Data;
                        user.Token = result.Data.RefreshToken;
                        _wrapper.Response(ref result, _wrapper.Response(await _context.UpdateSave(user)));
                    }
                    else
                        result = generateToken;
                }
                else
                    result.NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Login User", request);
                result.Error("Failed Add User", ex.Message);
            }
            return result;
        }
    }
}

