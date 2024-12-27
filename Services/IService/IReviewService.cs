using e_commerce.Models.DTOs.ReviewDTOs;
using e_commerce.Models.Entities;

namespace e_commerce.Services.IService
{
    public interface IReviewService
    {
        public Task<ICollection<Review>> GetByProductIdAsync(int  productID);
        public Task AddAsync(ReviewDTO model);
        public Task UpdateAsync(int ID, UpdateReviewDTO model);
        public Task DeleteAsync(int ID);
    }
}
