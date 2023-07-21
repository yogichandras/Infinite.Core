using AutoMapper;
using INFINITE.CORE.Data;
using INFINITE.CORE.Data.Base.Interface;
using INFINITE.CORE.Shared.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace INFINITE.CORE.Core.Identity.Role.Query
{
    public class GetRoleAllMapping : Profile
    {
        public GetRoleAllMapping()
        {
            CreateMap<INFINITE.CORE.Data.Model.Role, ReferensiStringObject>()
                .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.Nama, opt => opt.MapFrom(s => s.Name));
        }
    }

    public class GetRoleAllRequest : IRequest<ObjectResponse<List<ReferensiStringObject>>>
    {
    }

    internal class GetRoleAllHandler : IRequestHandler<GetRoleAllRequest, ObjectResponse<List<ReferensiStringObject>>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetRoleAllHandler(
            ILogger<GetRoleAllHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<ObjectResponse<List<ReferensiStringObject>>> Handle(GetRoleAllRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<List<ReferensiStringObject>> result = new ObjectResponse<List<ReferensiStringObject>>();
            try
            {
                var item = await _context.Entity<INFINITE.CORE.Data.Model.Role>().ToListAsync();
                result.Data = _mapper.Map<List<ReferensiStringObject>>(item);
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get All Role");
                result.Error("Failed Get All Role", ex.Message);
            }
            return result;
        }
    }
}
