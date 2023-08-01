using INFINITE.CORE.Core.Helper;
using INFINITE.CORE.Core.Request;
using INFINITE.CORE.Data;
using INFINITE.CORE.Data.Base.Interface;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Shared.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace INFINITE.CORE.Core.User.Command
{
    public class CreateUserRequest : UserInfoRequest,IRequest<StatusResponse>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public List<string> Roles { get; set; }
    }

    internal class CreateUserHandler : IRequestHandler<CreateUserRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public CreateUserHandler(
            ILogger<CreateUserHandler> logger,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _helper = helper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {

                if (_context.Entity<Data.Model.User>().Any(d => d.Username.ToLower() == request.Username.ToLower()))
                {
                    result.NotAcceptable("Username Sudah Terdaftar!");
                    return result;
                }
                if (!_helper.ValidatePassword(request.Password))
                {
                    result.NotAcceptable($"Password Must 8 Character Length and Contains Upper Case, Symbol and Numeric!");
                    return result;
                }

                string _hash_default_password = _helper.PasswordEncrypt(request.Password);
                Data.Model.User user = new Data.Model.User()
                {
                    Fullname = request.Fullname,
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    Mail = request.Mail,
                    PhoneNumber = request.PhoneNumber,
                    Password = _hash_default_password,
                    IsLockout = false,
                    AccessFailedCount = 0,
                    CreateBy = request.Username,
                    CreateDate = DateTime.Now,
                    UpdateBy = request.Username,
                    UpdateDate = DateTime.Now,
                };
                _context.Add(user);

                if (request.Roles != null && request.Roles.Count > 0)
                {
                    foreach (var item in request.Roles)
                    {
                        Data.Model.UserRole user_role = new Data.Model.UserRole()
                        {
                            CreateBy = request.Username,
                            CreateDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            IdRole = item,
                            IdUser = user.Id
                        };
                        _context.Add(user_role);
                    }
                }

                var save = await _context.Commit();
                if (save.Success)
                    result.OK();
                else
                    result.BadRequest(save.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Create User", request);
                result.Error("Failed Create User", ex.Message);
            }
            return result;
        }
    }
}

