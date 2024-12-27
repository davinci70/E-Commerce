using e_commerce.Models.DTOs.ReviewDTOs;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewSerive;

        public ReviewController(IReviewService reviewService)
        {
            _reviewSerive = reviewService;
        }

        [HttpGet("GetReviewsByProductId")]
        public async Task<IActionResult> GetByProductId(int ProductID)
        {
            try
            {
                var Reviews = await _reviewSerive.GetByProductIdAsync(ProductID);
                return Ok(Reviews);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview(ReviewDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _reviewSerive.AddAsync(model);
                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateReview")]
        public async Task<IActionResult> UpdateReview(int ID, UpdateReviewDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _reviewSerive.UpdateAsync(ID, model);
                return Ok("Review updated successfully.");
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteReview")]
        public async Task<IActionResult> DeleteReview(int ID)
        {
            try
            {
                await _reviewSerive.DeleteAsync(ID);
                return Ok("Review deleted successfully.");
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
