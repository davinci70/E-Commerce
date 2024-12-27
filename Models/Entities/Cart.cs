using e_commerce.Models.Users;

namespace e_commerce.Models.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<CartItem> cartItems { get; set; } = new List<CartItem>();
    }
}
