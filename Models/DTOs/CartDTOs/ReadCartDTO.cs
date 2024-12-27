using e_commerce.Models.Entities;
using e_commerce.Models.Users;

namespace e_commerce.Models.DTOs.CartDTOs
{
    public class ReadCartDTO
    {
        public int CartId { get; set; }
        public string CustomerID { get; set; }
        public ICollection<ReadCartItemDTO> cartItems { get; set; } = new List<ReadCartItemDTO>();
    }
}
