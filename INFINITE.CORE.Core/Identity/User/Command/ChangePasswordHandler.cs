using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Shared.Interface;

namespace INFINITE.CORE.Core.User.Command
{

    #region Request
    public class ChangePasswordRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
    #endregion

    internal class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public ChangePasswordHandler(
            ILogger<ChangePasswordHandler> logger,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _helper = helper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                string _old_password = _helper.PasswordEncrypt(request.Password.ToLower());
                string _new_password = _helper.PasswordEncrypt(request.NewPassword.ToLower());
                if (!_helper.ValidatePassword(request.NewPassword))
                {
                    result.NotAcceptable($"Password Must 8 Character Length and Contains Upper Case, Symbol and Numeric!");
                    return result;
                }

                var user = await _context.Entity<Data.Model.User>().Where(d => d.Id == request.UserId)
                                .OrderByDescending(d => d.CreateDate).FirstOrDefaultAsync();
                if (user != null)
                {
                    if(user.Password != _old_password)
                    {
                        result.NotAcceptable("Your old Password is incorrect!");
                        return result;
                    }
                    user.Password = _new_password;
                    user.UpdateBy = $"{user.Id}|{user.Fullname}";
                    user.UpdateDate = DateTime.Now;
                    var save = await _context.UpdateSave(user);
                    if (save.Success)
                        result.OK();
                    else
                        result.BadRequest(save.Message);
                }
                else
                    result.NotFound("User Not Found!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Change Password", request);
                result.Error("Failed Change Password", ex.Message);
            }
            return result;
        }
    }
}

