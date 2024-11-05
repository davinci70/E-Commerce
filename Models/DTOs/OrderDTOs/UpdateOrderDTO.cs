using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.OrderDTOs
{
    public class UpdateOrderDTO
    {
        [Required]
        public string ShippingAddress { get; set; }

        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
