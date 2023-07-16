using System;
using System.Collections.Generic;
using INFINITE.CORE.Data.Base.Interface;


namespace INFINITE.CORE.Data.Model 
{
    public partial class Notification : IEntity
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public string UserFullname { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Navigation { get; set; }
        public bool IsOpen { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
