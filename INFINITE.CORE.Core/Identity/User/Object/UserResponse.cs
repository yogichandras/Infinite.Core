//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoMapper;
using INFINITE.CORE.Core.Helper;
using INFINITE.CORE.Shared.Attributes;

namespace INFINITE.CORE.Core.Response
{
    public partial class UserResponse: IMapResponse<UserResponse, INFINITE.CORE.Data.Model.User>
    {
		public Guid Id{ get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Mail { get; set; }
        public string Nrk { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonnelNo { get; set; }
        public DateTime LastSynchronize { get; set; }
        public string Status { get; set; }
        public List<ReferensiStringObject> Roles { get; set; }

        public void Mapping(IMappingExpression<INFINITE.CORE.Data.Model.User, UserResponse> map)
        {
            //use this for mapping
            map.ForMember(d => d.Status, opt => opt.MapFrom(s => CheckStatus(s)));
            map.ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRole.Select(x => new ReferensiStringObject { Id = x.IdRoleNavigation.Id, Nama = x.IdRoleNavigation.Name }).ToList()));
        }
        private string CheckStatus(INFINITE.CORE.Data.Model.User s)
        {
            if (!s.Active)
                return "Not Active";
            if (s.IsLockout)
                return "Locked";
            else
                return "Active";
        }
    }
}

