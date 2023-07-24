using INFINITE.CORE.Data.Base.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INFINITE.CORE.Data.Base
{
    public class BaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string CreateByWithUserNameOnly { get { if (this.CreateBy != null) { if (this.CreateBy.Contains("|")) { return this.CreateBy.Split("|")[1]; } else { return this.CreateBy; } } else { return default; } } }
        public string UpdateByWithUserNameOnly { get { if (this.UpdateBy != null) { if (this.UpdateBy.Contains("|")) { return this.UpdateBy.Split("|")[1]; } else { return this.UpdateBy; } } else { return default; } } }
    }
    public class BaseGuidEntity : BaseEntity
    {
        public Guid Id { get; set; }
    }
    public class BaseIntEntity : BaseEntity
    {
        [Column(Order = 0)]
        public int Id { get; set; }
    }
    public class BaseStringEntity : BaseEntity
    {
        public string Id { get; set; }
    }
}
