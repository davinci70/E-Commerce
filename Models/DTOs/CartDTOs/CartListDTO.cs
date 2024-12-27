namespace e_commerce.Models.DTOs.CartDTOs
{
    public class CartListDTO
    {
        public string CustomerID { get; set; }
        public ICollection<ItemsToMoveDTO> CartList { get; set; } = new List<ItemsToMoveDTO>();
    }
}
