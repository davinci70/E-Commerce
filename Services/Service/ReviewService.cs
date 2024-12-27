using CloudinaryDotNet.Actions;
using e_commerce.Data;
using e_commerce.Models.DTOs.ReviewDTOs;
using e_commerce.Models.Entities;
using e_commerce.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Services.Service
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _Context;

        public ReviewService(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task AddAsync(ReviewDTO model)
        {
            if (NPLService.IsToxic(model.Body).Result
                || NPLService.IsProfane(model.Body)
                )
            {
                throw new ArgumentException("This review contains inappropriate language.");
            }

            var newReview = new Review()
            {
                CustomerID = model.CustomerID,
                ProductID = model.ProductID,
                Rating = model.Rating,
                Body = model.Body,
                ImagePath = "path",
                NSFWFlag = false,
                ReviewDate = DateTime.Now
            };

            try
            {
                await _Context.Reviews.AddAsync(newReview);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error: {ex.InnerException.Message}");
            }
        }

        public async Task DeleteAsync(int ID)
        {
            var Review = await _Context.Reviews.FindAsync(ID);

            if (Review == null)
            {
                throw new ValidationException($"Review is not found.");
            }

            _Context.Reviews.Remove(Review);
            await _Context.SaveChangesAsync();
        }

        public async Task<ICollection<Review>> GetByProductIdAsync(int productID)
        {
            var reviews = await _Context.Reviews
                .Where(r => r.ProductID == productID)
                .ToListAsync();

            if (!reviews.Any())
            {
                bool productExists = await _Context.Products
                    .AnyAsync(p => p.ProductID == productID);

                if (!productExists)
                {
                    throw new ValidationException("Invalid product ID.");
                }

                throw new ArgumentException("Product has no reviews yet.");
            }


            return reviews;
        }

        public async Task UpdateAsync(int ID, UpdateReviewDTO model)
        {
            var Review = await _Context.Reviews.FindAsync(ID);

            if (Review == null)
            {
                throw new ValidationException($"Review is not found.");
            }

            if (NPLService.IsProfane(model.Body))
            {
                throw new ArgumentException("This review contains inappropriate language.");
            }

            Review.Rating = model.Rating;
            Review.Body = model.Body;

            _Context.Reviews.Update(Review);
            await _Context.SaveChangesAsync();
        }
    }
}
