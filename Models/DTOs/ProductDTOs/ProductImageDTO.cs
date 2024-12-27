using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.ProductDTOs
{
    public class ProductImageDTO
    {       
        public int ProductImageID { get; set; }
        public string ImageUrl { get; set; }
    }
}
