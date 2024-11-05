namespace e_commerce.Models.DTOs.ProductDTOs
{
    public class UpdateProductDTO : ProductDTO
    {
        public List<int>? RemoveImagesIDs { get; set; }
    }
}
