using e_commerce.Models.DTOs;
using e_commerce.Models.Entities;
using e_commerce.Services.IService;

namespace e_commerce.Services.Service
{
    public class StoreService : IStoreService
    {
        public Task<int> AddAsync(StoreDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<Store> GetStoreById(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
