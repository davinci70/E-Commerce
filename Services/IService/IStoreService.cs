using e_commerce.Models.DTOs;
using e_commerce.Models.Entities;

namespace e_commerce.Services.IService
{
    public interface IStoreService
    {
        Task<Store> GetStoreById(int ID);
        Task<int> AddAsync(StoreDTO model);
    }
}
