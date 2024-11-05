using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.ProductDTOs
{
    public class ProductImageDTO
    {
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int ProductID { get; set; }
    }
}
