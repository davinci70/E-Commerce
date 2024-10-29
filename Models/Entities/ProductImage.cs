namespace e_commerce.Models.Entities
{
    public class ProductImage
    {
        public int ProductImageID { get; set; }
        public string ImageUrl { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
