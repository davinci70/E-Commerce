namespace e_commerce.Models.Entities.RefEntities
{
    public class RefProductType
    {
        public int ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
