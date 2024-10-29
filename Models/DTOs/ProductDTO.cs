using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs
{
    public class ProductDTO
    {
        [Required]
        public int ProductTypeID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }
        
        [Required, MaxLength(500)]
        public string SmallDescription { get; set; }

        [Required, Precision(15, 2)]
        public decimal Price { get; set; }
        
        [Required, Precision(15, 2)]
        public decimal Discount { get; set; }

        public List<IFormFile>? ProductImages { get; set; }

        [Required]
        public string SellerID { get; set; }
    }
}
