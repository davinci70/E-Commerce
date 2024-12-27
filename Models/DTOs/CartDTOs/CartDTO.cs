using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.CartDTOs
{
    public class CartDTO
    {
        public string CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
