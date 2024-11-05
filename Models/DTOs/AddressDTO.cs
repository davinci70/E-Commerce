using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs
{
    public class AddressDTO
    {
        //[Required]
        public string Street { get; set; }
        //[Required]
        public string City { get; set; }
        //[Required]
        public string State { get; set; }
        //[Required]
        public int PostalCode { get; set; }
        //[Required]
        public string Country { get; set; }
    }
}
