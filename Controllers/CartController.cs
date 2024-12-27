using e_commerce.Models.DTOs;
using e_commerce.Models.DTOs.CartDTOs;
using e_commerce.Models.Entities;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService CartService)
        {
            _cartService = CartService;
        }

        [HttpGet("GetCartByCustomerID")]
        public async Task<IActionResult> GetCartByCustomerID(string customerID)
        {
            try
            {
                var Cart = await _cartService.GetCartByCustomerID(customerID);
                return Ok(Cart);
            }
            catch (ArgumentException)
            {
                return NoContent();
            }
        }

        [HttpPost("AddOrUpdateCart")]
        public async Task<IActionResult> AddOrUpdateCartAsync(CartDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _cartService.AddOrUpdateCartAsync(model);
                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(string customerId, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _cartService.RemoveFromCartAsync(customerId, productId);
                return Ok("Item deleted successfuly.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart(string customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _cartService.ClearCartAsync(customerId);
                return Ok("Cart deleted successfuly.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("MoveAllCart")]
        public async Task<IActionResult> MoveAllCart(CartListDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _cartService.MoveAllCartAsync(model);
                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
