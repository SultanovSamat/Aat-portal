using System.ComponentModel.DataAnnotations;

namespace AkfortaWeb.Models
{
    public class MemberLoginModel
    {
        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}