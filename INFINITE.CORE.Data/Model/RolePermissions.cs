using System;
using System.Collections.Generic;
using INFINITE.CORE.Data.Base.Interface;


namespace INFINITE.CORE.Data.Model 
{
    public partial class RolePermissions : IEntity
    {
        public Guid Id { get; set; }
        public string IdRole { get; set; }
        public string Permission { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
    }
}
