using e_commerce.Models.Entities;

namespace e_commerce.Models.DTOs.OrderDTOs
{
    public class ReadOrderDTO
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName {  get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
