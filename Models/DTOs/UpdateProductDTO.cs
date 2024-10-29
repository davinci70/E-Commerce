namespace e_commerce.Models.DTOs
{
    public class UpdateProductDTO : ProductDTO
    {
        public List<int>? RemoveImagesIDs { get; set; }
    }
}
