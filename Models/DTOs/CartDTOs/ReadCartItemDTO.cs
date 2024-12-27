using e_commerce.Models.Entities;

namespace e_commerce.Models.DTOs.CartDTOs
{
    public class ReadCartItemDTO
    {
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}
