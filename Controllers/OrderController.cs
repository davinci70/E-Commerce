using e_commerce.Helpers;
using e_commerce.Models.DTOs.OrderDTOs;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(OrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _orderService.AddAsync(model);
                return Ok("Order placed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int ID, UpdateOrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _orderService.UpdateAsync(ID, model);
                return Ok("Order updated successfully.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int OrderID, OrderStatus.enOrderStatus orderStatus)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(OrderID, orderStatus);
                return Ok("Order status updated successfully.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetCustomerOrders")]
        public async Task<IActionResult> GetCustomerOrders(string CustomerID)
        {
            try
            {
                var Orders = await _orderService.GetCustomerOrdersAsync(CustomerID);
                return Ok(Orders);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
