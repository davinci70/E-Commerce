using e_commerce.Helpers;
using e_commerce.Models.DTOs.OrderDTOs;
using e_commerce.Models.Users;
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

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult> GetAllOrders()
        {
            try
            {
                var Orders = await _orderService.GetAllOrdersAsync();
                return Ok(Orders);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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
                int OrderID = await _orderService.AddAsync(model);
                return Ok(new { OrderID });
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

        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(int OrderID)
        {
            try
            {
                var Orders = await _orderService.GetOrderByIdAsync(OrderID);
                return Ok(Orders);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
