//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoMapper;
using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Core.Helper;
using INFINITE.CORE.Core.Request;

namespace INFINITE.CORE.Core.User.Command
{

    #region Request
    public class AddUserMapping: Profile
    {
        public AddUserMapping()
        {
            CreateMap<AddUserRequest, UserRequest>().ReverseMap();
        }
    }
    public class AddUserRequest :UserRequest, IMapRequest<INFINITE.CORE.Data.Model.User, AddUserRequest>,IRequest<StatusResponse>
    {
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<AddUserRequest, INFINITE.CORE.Data.Model.User> map)
        {
            //use this for mapping
            //map.ForMember(d => d.EF_COLUMN, opt => opt.MapFrom(s => s.Object));
        }
    }
    #endregion

    internal class AddUserHandler : IRequestHandler<AddUserRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public AddUserHandler(
            ILogger<AddUserHandler> logger,
            IMapper mapper,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }
        public async Task<StatusResponse> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var data = _mapper.Map<INFINITE.CORE.Data.Model.User>(request);
                data.CreateBy = request.Inputer;
                data.CreateDate = DateTime.Now;
                var add = await _context.AddSave(data);
                if (add.Success)
                    result.OK();
                else
                    result.BadRequest(add.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Add User", request);
                result.Error("Failed Add User", ex.Message);
            }
            return result;
        }
    }
}

