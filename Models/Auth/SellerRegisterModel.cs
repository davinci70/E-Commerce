using e_commerce.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.Auth
{
    public class SellerRegisterModel
    {
        //[Required]
        public UserRegisterModel userRegister { get; set; }
        //[Required]
        public StoreDTO store { get; set; }
    }
}
