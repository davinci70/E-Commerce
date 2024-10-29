using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs
{
    public class StoreDTO
    {
        [Required]
        public string StoreName { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
