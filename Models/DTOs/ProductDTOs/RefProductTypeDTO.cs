using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.ProductDTOs
{
    public class RefProductTypeDTO
    {
        [Required]
        public string ProductTypeName { get; set; }
    }
}
