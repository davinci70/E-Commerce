using e_commerce.Models.DTOs.ProductDTOs;
using e_commerce.Models.Entities;

namespace e_commerce.Services.IService
{
    public interface IProductService
    {
        public Task<ReadProductDTO> GetByIdAsync(int ID);
        public Task<ICollection<ReadProductDTO>> GetAllAsync();
        public Task AddAsync(ProductDTO model);
        public Task DeleteAsync(int ID);
        public Task UpdateAsync(int ProductID, UpdateProductDTO model);
        public Task<PagedResult<ReadProductDTO>> SearchProductAsync(ProductSearch productSearch);
    }
}
