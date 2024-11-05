namespace e_commerce.Models.Entities
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
