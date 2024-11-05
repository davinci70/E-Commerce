using e_commerce.Models.DTOs.ProductDTOs;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefProductTypeController : ControllerBase
    {
        private readonly IRefProductTypeService _refProductTypeService;

        public RefProductTypeController(IRefProductTypeService refProductTypeService)
        {
            _refProductTypeService = refProductTypeService;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var productTypes = await _refProductTypeService.GetAllAsync();
                return Ok(productTypes);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GeyByID")]
        public async Task<IActionResult> GetByID(int ID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var refProductionType = await _refProductTypeService.GetByIdAsync(ID);
                return Ok(refProductionType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException  ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(RefProductTypeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _refProductTypeService.AddAsync(model);
            return Ok(model);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int ID, RefProductTypeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _refProductTypeService.UpdateAsync(ID, model);
                return Ok("Product type updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int ID)
        {
            try
            {
                await _refProductTypeService.DeleteAsync(ID);
                return Ok("Product type deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
