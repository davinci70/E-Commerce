using e_commerce.Models.Users;

namespace e_commerce.Models.Entities
{
    public class Store
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }

        public string SellerID { get; set; } 
        public Seller Seller { get; set; }
    }
}
