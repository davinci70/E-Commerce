using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.ReviewDTOs
{
    public class UpdateReviewDTO
    {
        [Required]
        [Range(1, 5, ErrorMessage = "This value must be between 1 and 5.")]
        public byte Rating { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
