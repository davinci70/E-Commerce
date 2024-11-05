using e_commerce.Models.DTOs.ProductDTOs;
using e_commerce.Models.Entities.RefEntities;

namespace e_commerce.Services.IService
{
    public interface IRefProductTypeService
    {
        public Task<IEnumerable<RefProductType>> GetAllAsync();
        public Task<RefProductType> GetByIdAsync(int ID);
        public Task AddAsync(RefProductTypeDTO model);
        public Task UpdateAsync(int ID, RefProductTypeDTO model);
        public Task DeleteAsync(int ID);
    }
}
