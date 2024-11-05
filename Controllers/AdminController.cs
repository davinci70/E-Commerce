using e_commerce.Models.Auth;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _AdminService;

        public AdminController(IAdminService AdminService)
        {
            _AdminService = AdminService;
        }

        [HttpGet("GetByID")]
        public async Task<IActionResult> GetByID(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Admin = await _AdminService.GetByIdAsync(id);
                return Ok(Admin);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException  ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Admins = await _AdminService.GetAllAsync();
                return Ok(Admins);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AdminRegister")]
        public async Task<IActionResult> AdminRegister( AdminRegisterModel adminModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _AdminService.AdminRegisterAsync(adminModel);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin(string ID, AdminRegisterModel adminModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _AdminService.UpdateAdminAsync(ID, adminModel);
                return Ok(adminModel);
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

        [HttpDelete("DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin(string ID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _AdminService.DeleteAdminAsync(ID);
                return Ok("Admin deleted successfully.");
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
    }
}
