using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs
{
    public class ProductImageDTO
    {
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int ProductID { get; set; }
    }
}
