using System;
using System.Collections.Generic;
using INFINITE.CORE.Data.Base.Interface;


namespace INFINITE.CORE.Data.Model 
{
    public partial class User : ISoftEntity
    {
        public User()
        {
            Notification = new HashSet<Notification>();
            UserRole = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public bool IsLockout { get; set; }
        public int AccessFailedCount { get; set; }
        public bool Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
