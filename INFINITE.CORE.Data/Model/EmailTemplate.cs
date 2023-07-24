using System;
using System.Collections.Generic;
using INFINITE.CORE.Data.Base.Interface;


namespace INFINITE.CORE.Data.Model 
{
    public partial class EmailTemplate : IEntity
    {
        public Guid Id { get; set; }
        public string Module { get; set; }
        public string MailFrom { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string MailContent { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
