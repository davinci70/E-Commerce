using e_commerce.Models.Users;

namespace e_commerce.Models.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public byte Rating { get; set; }    
        public string Body { get; set; }
        public string? ImagePath { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool NSFWFlag { get; set; }
    }
}
