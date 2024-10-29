using e_commerce.Models.DTOs;
using e_commerce.Models.Entities;

namespace e_commerce.Services.IService
{
    public interface IAddressService
    {
        Task<Address> GetByIdAsync(int ID);
        Task<int> AddAsync(AddressDTO model);
    }
}
