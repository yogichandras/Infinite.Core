using INFINITE.CORE.Core.Helper;
using INFINITE.CORE.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace INFINITE.CORE.Core.Request
{
    public partial class RepositoryRequest
    {
        [Required]
        public ModulType Modul { get; set; }
        [Required]
        public FileObject File { get; set; }
        [Required]
		public string Code{ get; set; }
		public string Description{ get; set; }
		[Required]
		public bool IsPublic{ get; set; }

    }
}

