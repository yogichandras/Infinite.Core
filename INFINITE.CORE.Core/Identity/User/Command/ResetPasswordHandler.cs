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

namespace INFINITE.CORE.Core.User.Command
{
    #region Request
    public class ResetPasswordRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IWrapperHelper _wrapper;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public ResetPasswordHandler(
            ILogger<ResetPasswordHandler> logger,
            IWrapperHelper wrapper,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _wrapper = wrapper;
            _helper = helper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                string default_password = "default_password";
                var user = await _context.Entity<Data.Model.User>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                string _hash_default_password = _helper.PasswordEncrypt(default_password);
                if (user != null)
                {
                    user.Password = _hash_default_password;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateBy = request.Inputer;
                    result = _wrapper.Response(await _context.UpdateSave(user));
                }
                else
                    result.NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Reset Password", request);
                result.Error("Failed Reset Password", ex.Message);
            }
            return result;
        }
    }
}

