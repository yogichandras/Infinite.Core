using System;
using System.Collections.Generic;
using INFINITE.CORE.Data.Base.Interface;


namespace INFINITE.CORE.Data.Model 
{
    public partial class Role : ISoftEntity
    {
        public Role()
        {
            RolePermissions = new HashSet<RolePermissions>();
            UserRole = new HashSet<UserRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
