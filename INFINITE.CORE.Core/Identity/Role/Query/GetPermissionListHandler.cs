using INFINITE.CORE.Core.Identity.Role.Object;
using INFINITE.CORE.Data.Provider;
using INFINITE.CORE.Shared.Attributes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace INFINITE.CORE.Core.Identity.Role.Query
{
    public class GetPermissionListRequest : IRequest<ObjectResponse<PermissionResponse>>
    {
    }
    
    internal class GetPermissionListHandler : IRequestHandler<GetPermissionListRequest, ObjectResponse<PermissionResponse>>
    {
        private readonly ILogger _logger;
        public GetPermissionListHandler(ILogger<GetPermissionListHandler> logger)
        {
            _logger = logger;
        }

        public async Task<ObjectResponse<PermissionResponse>> Handle(GetPermissionListRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<PermissionResponse> result = new ObjectResponse<PermissionResponse>();
            try
            {
                result.Data = new PermissionResponse
                {
                    Permissions = Permissions.List()
                };
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List Permissions", request);
                result.Error("Failed Get List Permissions", ex.Message);
            }
            
            return result;
        }
    }
}
