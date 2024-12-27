namespace e_commerce.Models.DTOs.CartDTOs
{
    public class CartItemDTO
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
