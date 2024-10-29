using e_commerce.Models.Auth;
using e_commerce.Models.DTOs.UserDTOs;

namespace e_commerce.Services.IService
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetByIdAsync(string ID);
        Task<IEnumerable<CustomerDTO>> GetAllAsync();
        Task<AuthModel> CustomerRegisterAsync(CustomerRegisterModel Model);
        Task UpdateCustomerAsync(string ID, CustomerRegisterModel Model);
        Task DeleteCustomerAsync(string ID);
    }
}
