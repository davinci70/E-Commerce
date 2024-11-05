using e_commerce.Models.Auth;
using e_commerce.Models.DTOs.UserDTOs;

namespace e_commerce.Services.IService
{
    public interface ISellerService
    {
        Task<SellerDTO> GetByIdAsync(string ID);
        Task<IEnumerable<SellerDTO>> GetAllAsync();
        Task<AuthModel> SellerRegisterAsync(SellerRegisterModel Model);
        Task UpdateSellerAsync(string ID, SellerRegisterModel Model);
        Task DeleteSellerAsync(string ID);
    }
}
