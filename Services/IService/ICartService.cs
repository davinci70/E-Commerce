using e_commerce.Models.DTOs;
using e_commerce.Models.DTOs.CartDTOs;
using e_commerce.Models.Entities;

namespace e_commerce.Services.IService
{
    public interface ICartService
    {
        public Task<ReadCartDTO> GetCartByCustomerID(string customerID);
        public Task AddOrUpdateCartAsync(CartDTO model);
        public Task RemoveFromCartAsync(string customerId, int productId);
        public Task ClearCartAsync(string customerId);
        public Task MoveAllCartAsync(CartListDTO models);
    }
}
