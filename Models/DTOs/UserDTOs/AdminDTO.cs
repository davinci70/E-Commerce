using e_commerce.Models.Entities;

namespace e_commerce.Models.DTOs.UserDTOs
{
    public class AdminDTO
    {
        public string AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
        public string AdminNotes { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
