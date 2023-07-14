using System.ComponentModel.DataAnnotations;
using INFINITE.CORE.Shared.Attributes;

namespace INFINITE.CORE.Core.Request
{
    public class UserInfoRequest
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
    }
}
