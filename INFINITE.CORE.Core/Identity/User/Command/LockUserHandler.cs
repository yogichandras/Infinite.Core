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
    public class LockUserRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool Value { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class LockUserHandler : IRequestHandler<LockUserRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IWrapperHelper _wrapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public LockUserHandler(
            ILogger<LockUserHandler> logger,
            IWrapperHelper wrapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _wrapper = wrapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(LockUserRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var user = await _context.Single(
                                    _context.Entity<Data.Model.User>()
                                    .Where(d => d.Id == request.Id)
                                );
                if (user != null)
                {
                    user.IsLockout = request.Value;
                    if (!user.IsLockout)
                        user.AccessFailedCount = 0;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateBy = request.Inputer;
                    result = _wrapper.Response(await _context.UpdateSave(user));
                }
                else
                    result.NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Lock Request", request);
                result.Error("Failed Lock Request", ex.Message);
            }
            return result;
        }
    }
}

