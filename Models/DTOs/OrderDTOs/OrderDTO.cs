using e_commerce.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace e_commerce.Models.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        [Required]
        public string CustomerID { get; set; }
        [Required]
        public string ShippingAddress { get; set; }

        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
