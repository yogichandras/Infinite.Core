using AutoMapper;
using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Core.Request;

namespace INFINITE.CORE.Core.User.Command
{
    #region Request
    public class EditInfoUserRequest : UserInfoRequest, IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
    public class EditInfoUserMapping : Profile
    {
        public EditInfoUserMapping()
        {
            CreateMap<EditInfoUserRequest, UserInfoRequest>().ReverseMap();
        }
    }
    #endregion

    internal class EditInfoUserHandler : IRequestHandler<EditInfoUserRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public EditInfoUserHandler(
            ILogger<EditInfoUserHandler> logger,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _context = context;
        }
        public async Task<StatusResponse> Handle(EditInfoUserRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var user = await _context.Entity<Data.Model.User>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Mail = request.Mail;
                    user.PhoneNumber = request.PhoneNumber;
                    user.Fullname = request.Fullname;
                    user.UpdateBy = user.Username;
                    user.UpdateDate = DateTime.Now;

                    var update = await _context.UpdateSave(user);
                    if (update.Success)
                        result.OK();
                    else
                        result.BadRequest(update.Message);

                    return result;
                }
                else
                    result.NotFound($"Id User {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Edit User", request);
                result.Error("Failed Edit User", ex.Message);
            }
            return result;
        }
    }
}

