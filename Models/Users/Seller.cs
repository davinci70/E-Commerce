using e_commerce.Models.Entities;

namespace e_commerce.Models.Users
{
    public class Seller : ApplicationUser
    {
        //public int StoreID { get; set; }
        public Store Store { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        //public List<SalesReport> SalesReports { get; set; }      
        //public List<PaymentMethod> PaymentMethods { get; set; }
        //public List<Payout> PayoutHistory { get; set; }
    }
}
