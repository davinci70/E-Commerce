using e_commerce.Models.Entities;
using e_commerce.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.ReviewDTOs
{
    public class ReviewDTO
    {
        public string CustomerID { get; set; }
        public int ProductID { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "This value must be between 1 and 5.")]
        public byte Rating { get; set; }
        [Required]
        public string Body { get; set; }
        //public string? ImagePath { get; set; }
        //[Required]
        //public bool NSFWFlag { get; set; }
    }
}
