namespace e_commerce.Models.Users
{
    public class Admin : ApplicationUser
    {       
        public DateTime LastLogin { get; set; }
        public string? AdminNotes { get; set; }
    }
}
