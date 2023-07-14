using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Shared.Interface;

namespace INFINITE.CORE.Core.User.Command
{

    #region Request
    public class RefreshTokenRequest : IRequest<ObjectResponse<TokenObject>>
    {
        [Required]
        public string Token { get; set; }
    }
    #endregion

    internal class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, ObjectResponse<TokenObject>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IWrapperHelper _wrapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public RefreshTokenHandler(
            ILogger<RefreshTokenHandler> logger,
            IMediator mediator,
            IWrapperHelper wrapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _wrapper = wrapper;
            _context = context;
        }
        public async Task<ObjectResponse<TokenObject>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<TokenObject> result = new ObjectResponse<TokenObject>();
            try
            {
                var user = await _context.Single(
                                    _context.Entity<Data.Model.User>()
                                    .Where(d => d.Token == request.Token)
                                );
                if (user != null)
                {
                    user.UpdateDate = DateTime.Now;
                    user.UpdateBy = user.Username;
                    var generateToken = await _mediator.Send(new GenerateTokenRequest()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = user.Fullname,
                        Mail = user.Mail,
                        Role = user.UserRole.Select(d => new ReferensiStringObject()
                        {
                            Id = d.IdRole,
                            Nama = d.IdRoleNavigation.Name
                        }).ToList(),
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
                _logger.LogError(ex, "Failed Refresh Token", request);
                result.Error("Failed Refresh Token", ex.Message);
            }
            return result;
        }
    }
}

