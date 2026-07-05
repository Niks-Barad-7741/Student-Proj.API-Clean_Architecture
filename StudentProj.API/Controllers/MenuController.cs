using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var items = await _service.GetAllMenusAsync();
            var response = ApiResponse<IEnumerable<MenuDTO>>.SuccessResponse(items, "Menus retrieved successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var menu = await _service.GetMenuByIdAsync(id);
            if (menu == null) return NotFound(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Menu not found"));
            var response = ApiResponse<MenuDTO>.Create(ResponseStatus.MenuRetriveSuccessfully, menu);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMenuDTO dto)
        {
            var exists = await _service.MenuExistsAsync(dto.MenuName);
            if (exists)
                return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "A menu with this name already exists"));

            var created = await _service.CreateMenuAsync(dto);
            var response = ApiResponse<object>.SuccessResponse(created, "Menu Added Successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuDTO dto)
        {
            var (success, error) = await _service.UpdateMenuAsync(id, dto);
            if (!success) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, error ?? "Failed to update menu"));
            var response = ApiResponse<object>.SuccessResponse(null, "Menu Updated Successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var res = await _service.DeleteMenuAsync(id);
            if (!res) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete menu"));
            var response = ApiResponse<object>.SuccessResponse(null, "Menu Deleted Successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("My-Menus")]
        public async Task<IActionResult> GetMyMenus()
        {
            var userIdStr = HttpContext.User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var roles = HttpContext.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            var menus = await _service.GetMenusFromUserAsync(userId, roles);

            var response = ApiResponse<List<MenuDTO>>.Create(ResponseStatus.MenuRetriveSuccessfully, menus);
            return StatusCode(response.StatusCodes, response);
        }
    }
}
