using e_commerce.Models.DTOs;
using e_commerce.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.Auth
{
    public class UserRegisterModel
    {
        [Required, StringLength(30)]
        public string FirstName { get; set; }

        [Required, StringLength(30)]
        public string LastName { get; set; }

        [Required, StringLength(30)]
        public string Username { get; set; }

        [Required, StringLength(128)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required, StringLength(50)]
        public string Password { get; set; }

        //[Required]
        public AddressDTO Address { get; set; }
        //[Required]
        public DateTime DateCreated { get; set; }
        //[Required]
        public bool IsActive { get; set; }
    }
}
