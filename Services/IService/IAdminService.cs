using e_commerce.Models.Auth;
using e_commerce.Models.DTOs.UserDTOs;
using e_commerce.Models.Users;

namespace e_commerce.Services.IService
{
    public interface IAdminService
    {
        Task<AdminDTO> GetByIdAsync(string ID);
        Task<IEnumerable<AdminDTO>> GetAllAsync();
        Task<AuthModel> AdminRegisterAsync(AdminRegisterModel Model);
        Task UpdateAdminAsync(string ID, AdminRegisterModel Model);
        Task DeleteAdminAsync(string ID);
    }
}
