//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using INFINITE.CORE.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using INFINITE.CORE.Data.Model;

namespace INFINITE.CORE.Core.Response
{
    public partial class UserResponse: IMapResponse<UserResponse, INFINITE.CORE.Data.Model.User>
    {
		public Guid Id{ get; set; }
		public int AccessFailedCount{ get; set; }
		public bool Active{ get; set; }
		public string CreateBy{ get; set; }
		public DateTime CreateDate{ get; set; }
		public string Fullname{ get; set; }
		public bool IsLockout{ get; set; }
		public string Mail{ get; set; }
		public string Password{ get; set; }
		public string PhoneNumber{ get; set; }
		public string Token{ get; set; }
		public string UpdateBy{ get; set; }
		public DateTime? UpdateDate{ get; set; }
		public string Username{ get; set; }


        public void Mapping(IMappingExpression<INFINITE.CORE.Data.Model.User, UserResponse> map)
        {
            //use this for mapping
            //map.ForMember(d => d.object, opt => opt.MapFrom(s => s.EF_COLUMN));

        }
    }
}

