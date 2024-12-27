namespace e_commerce.Models.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        public int ProductID { get; set; }
        public decimal ProductPrice { get; set; }
        public short Quantity { get; set; }
    }
}
