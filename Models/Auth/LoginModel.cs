using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.Auth
{
    public class LoginModel
    {
        [Required, StringLength(30)]
        public string Username { get; set; }

        [Required, StringLength(50)]
        public string Password { get; set; }
    }
}
