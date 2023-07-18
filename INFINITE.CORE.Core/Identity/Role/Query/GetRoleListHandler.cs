//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoMapper;
using MediatR;
using INFINITE.CORE.Data.Base.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Data;
using INFINITE.CORE.Data.Model;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Core.Response;
using INFINITE.CORE.Core.Helper;

namespace INFINITE.CORE.Core.Role.Query
{
    public class GetRoleListRequest : ListRequest,IListRequest<GetRoleListRequest>,IRequest<ListResponse<RoleResponse>>
    {
    }
    internal class GetRoleListHandler : IRequestHandler<GetRoleListRequest, ListResponse<RoleResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetRoleListHandler(
            ILogger<GetRoleListHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ListResponse<RoleResponse>> Handle(GetRoleListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<RoleResponse> result = new ListResponse<RoleResponse>();
            try
            {
				var query = _context.Entity<INFINITE.CORE.Data.Model.Role>().Include(x => x.RolePermissions).AsQueryable();

				#region Filter
				Expression<Func<INFINITE.CORE.Data.Model.Role, object>> column_sort = null;
				List<Expression<Func<INFINITE.CORE.Data.Model.Role, bool>>> where = new List<Expression<Func<INFINITE.CORE.Data.Model.Role, bool>>>();
				if (request.Filter != null && request.Filter.Count > 0)
				{
					foreach (var f in request.Filter)
					{
						var obj = ListExpression(f.Search, f.Field, true);
						if (obj.where != null)
							where.Add(obj.where);
					}
				}
				if (where != null && where.Count() > 0)
				{
					foreach (var w in where)
					{
						query = query.Where(w);
					}
				}
				if (request.Sort != null)
                {
					column_sort = ListExpression(request.Sort.Field, request.Sort.Field, false).order!;
					if(column_sort != null)
						query = request.Sort.Type == SortTypeEnum.ASC ? query.OrderBy(column_sort) : query.OrderByDescending(column_sort);
					else
						query = query.OrderBy(d=>d.Id);
				}
				#endregion

				var query_count = query;
				if (request.Start.HasValue && request.Length.HasValue && request.Length > 0)
					query = query.Skip((request.Start.Value - 1) * request.Length.Value).Take(request.Length.Value);
				var data_list = await query.ToListAsync();

				result.List = data_list.Select(x =>
				{
					var role = _mapper.Map<RoleResponse>(x);
					role.Permissions = x.RolePermissions.Select(x => x.Permission).ToList();
					return role;
                }).ToList();
				result.Filtered = data_list.Count();
				result.Count = await query_count.CountAsync();
				result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List Role", request);
                result.Error("Failed Get List Role", ex.Message);
            }
            return result;
        }

        #region List Utility
		private (Expression<Func<INFINITE.CORE.Data.Model.Role, bool>> where, Expression<Func<INFINITE.CORE.Data.Model.Role, object>> order) ListExpression(string search, string field, bool is_where)
		{
			Expression<Func<INFINITE.CORE.Data.Model.Role, object>> result_order = null;
			Expression<Func<INFINITE.CORE.Data.Model.Role, bool>> result_where = null;
            if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(field))
            {
                field = field.Trim().ToLower();
                search = search.Trim().ToLower();
                switch (field)
                {
					case "id" : 
						if(is_where)
								result_where = (d=>d.Id == search);
						else
							result_order = (d => d.Id);
					break;
					case "active" : 
						if(is_where){
							if (bool.TryParse(search, out var _Active))
								result_where = (d=>d.Active == _Active);
						}
						else
							result_order = (d => d.Active);
					break;
					case "createby" : 
						if(is_where){
							result_where = (d=>d.CreateBy.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.CreateBy);
					break;
					case "createdate" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _CreateDate))
								result_where = (d=>d.CreateDate == _CreateDate);
						}
						else
							result_order = (d => d.CreateDate);
					break;
					case "name" : 
						if(is_where){
							result_where = (d=>d.Name.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Name);
					break;
					case "updateby" : 
						if(is_where){
							result_where = (d=>d.UpdateBy.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.UpdateBy);
					break;
					case "updatedate" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _UpdateDate))
								result_where = (d=>d.UpdateDate == _UpdateDate);
						}
						else
							result_order = (d => d.UpdateDate);
					break;
                }
            }
            return (result_where, result_order);
        }
        #endregion
    }
}

