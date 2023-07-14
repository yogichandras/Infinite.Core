using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Shared.Interface;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace INFINITE.CORE.Core.Notification.Command
{
    #region Request
    public class PushNotifMainAppRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid IdUser { get; set; }
    }
    #endregion

    internal class PushNotifMainAppHandler : IRequestHandler<PushNotifMainAppRequest, StatusResponse>
    {
        private readonly ILogger _Logger;
        private readonly IHttpRequest _HttpRequest;
        private string _AppBaseUrl;
        public PushNotifMainAppHandler(
            ILogger<PushNotifMainAppHandler> logger,
            IHttpRequest httpRequest,
            IConfiguration configuration)
        {
            _Logger = logger;
            _HttpRequest = httpRequest;
            _AppBaseUrl = configuration.GetValue<string>("AppBaseUrl");
        }

        public async Task<StatusResponse> Handle(PushNotifMainAppRequest request, CancellationToken cancellationToken)
        {
            var result = new StatusResponse();
            try
            {
                var x = await _HttpRequest.DoRequestData<string>(HttpMethod.Post, null, $"{_AppBaseUrl}api/Notification/push_notif", request);
                result.OK();
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Error at PushNotifMainAppHandler", request);
                result.Error("Error at PushNotifMainAppHandler", ex.ToString());
            }
            return result;
        }
    }
}
