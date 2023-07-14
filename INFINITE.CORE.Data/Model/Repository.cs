using INFINITE.CORE.Data.Base.Interface;
using System;
using System.Collections.Generic;


namespace INFINITE.CORE.Data.Model 
{
    public partial class Repository : IEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Modul { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public bool IsPublic { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
