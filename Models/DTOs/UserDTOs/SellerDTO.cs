namespace e_commerce.Models.DTOs.UserDTOs
{
    public class SellerDTO
    {
        public string SellerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public StoreDTO Store { get; set; }
    }
}
