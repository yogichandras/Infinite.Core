//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace INFINITE.CORE.Core.Request
{
    public partial class RoleRequest
    {
        [Required]
        public string Id { get; set; }
        [Required]
		public bool Active{ get; set; }
		[Required]
		public string Name{ get; set; }
        [Required]
        public List<string> Permissions { get; set; }
    }
}

