using e_commerce.Models.Entities;

namespace e_commerce.Models.Users
{
    public class Customer : ApplicationUser
    {
        //public ICollection<Product> WhishList { get; set; } = new List<Product>();
        //public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public Cart Cart { get; set; }
    }
}
