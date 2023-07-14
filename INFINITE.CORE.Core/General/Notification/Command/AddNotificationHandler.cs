using AutoMapper;
using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Core.Helper;
using INFINITE.CORE.Core.Request;
using Hangfire.Common;
using Hangfire;
using INFINITE.CORE.Shared.Interface;
using INFINITE.CORE.Infrastructure.Mail.Interface;


namespace INFINITE.CORE.Core.Notification.Command
{

    #region Request
    public class AddNotificationMapping: Profile
    {
        public AddNotificationMapping()
        {
            CreateMap<AddNotificationRequest, NotificationRequest>().ReverseMap();
        }
    }
    public class AddNotificationRequest :NotificationRequest, IMapRequest<Data.Model.Notification, AddNotificationRequest>,IRequest<ObjectResponse<Guid>>
    {
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<AddNotificationRequest, Data.Model.Notification> map)
        {
            //use this for mapping
            //map.ForMember(d => d.EF_COLUMN, opt => opt.MapFrom(s => s.Object));
        }
    }
    #endregion

    internal class AddNotificationHandler : IRequestHandler<AddNotificationRequest, ObjectResponse<Guid>>
    {

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IEmailService _mail;
        private readonly IHttpRequest _httpRequest;
        private readonly IBackgroundJobClient _job;
        private readonly IUnitOfWork<ApplicationDBContext> _context;

        public AddNotificationHandler(
            ILogger<AddNotificationHandler> logger,
            IMapper mapper,
            IMediator mediator,
            IEmailService mail,
            IHttpRequest httpRequest,
            IBackgroundJobClient job,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _mail = mail;
            _httpRequest = httpRequest;
            _job = job;
            _context = context;
        }
        public async Task<ObjectResponse<Guid>> Handle(AddNotificationRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<Guid> result = new ObjectResponse<Guid>();
            try
            {
                var data = _mapper.Map<Data.Model.Notification>(request);
                data.IsOpen = false;
                data.CreateBy = request.Inputer;
                data.CreateDate = DateTime.Now;
                data.Id = Guid.NewGuid();
                var add = await _context.AddSave(data);
                if (add.Success)
                {
                    if (!string.IsNullOrWhiteSpace(request.UserMail))
                    {
                        _job.Enqueue(() =>
                                        _mail.SendMail(
                                            new List<string>() { request.UserMail }, null,
                                            request.Subject,
                                            request.Description, null
                                        ));
                    }
                    result.Data = data.Id;
                    result.OK();

                    #region Push notif
                    _ = _mediator.Send(new PushNotifMainAppRequest
                    {
                        IdUser = request.IdUser
                    });
                    #endregion
                }
                else
                    result.BadRequest(add.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Add Notification", request);
                result.Error("Failed Add Notification", ex.Message);
            }
            return result;
        }
    }
}

