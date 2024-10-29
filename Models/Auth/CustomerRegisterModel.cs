using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.Auth
{
    public class CustomerRegisterModel
    {
        //[Required]
        public UserRegisterModel userRegister { get; set; }
    }
}
