using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs
{
    public class RefProductTypeDTO
    {
        [Required]
        public string ProductTypeName { get; set; }
    }
}
