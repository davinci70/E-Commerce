using e_commerce.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.Auth
{
    public class AdminRegisterModel
    {
        //[Required]
        public UserRegisterModel userRegister { get; set; }
        //[Required]
        public DateTime LastLogin { get; set; }
        //[Required]
        public string? AdminNotes { get; set; }
    }
}
