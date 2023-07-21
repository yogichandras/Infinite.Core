using AutoMapper;
using INFINITE.CORE.Core.Request;
using INFINITE.CORE.Data;
using INFINITE.CORE.Data.Base.Interface;
using INFINITE.CORE.Shared.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace INFINITE.CORE.Core.User.Command
{
    #region Request
    public class EditInfoUserRequest : UserInfoRequest, IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public List<string> Roles { get; set; }
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
                var user = await _context.Entity<Data.Model.User>().Include(x => x.UserRole).Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Mail = request.Mail;
                    user.PhoneNumber = request.PhoneNumber;
                    user.Fullname = request.Fullname;
                    user.UpdateBy = user.Username;
                    user.UpdateDate = DateTime.Now;

                    user.UserRole.Clear();
                    foreach (var item in request.Roles)
                    {
                        user.UserRole.Add(new Data.Model.UserRole
                        {
                            CreateBy = user.Username,
                            CreateDate = DateTime.Now,
                            IdRole = item,
                            IdUser = user.Id
                        });
                    }

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

