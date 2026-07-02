using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuController : Controller
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
            var response = ApiResponse<IEnumerable<MenuDTO>>.Create(ResponseStatus.UserRetriveSuccessfully, items);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuDTO dto)
        {
            var created = await _service.CreateMenuAsync(dto);
            var response = ApiResponse<object>.Create(ResponseStatus.UserAddedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuDTO dto)
        {
            var res = await _service.UpdateMenuAsync(id, dto);
            if (!res) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to update menu"));
            var response = ApiResponse<object>.Create(ResponseStatus.UserUpdatedSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var res = await _service.DeleteMenuAsync(id);
            if (!res) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete menu"));
            var response = ApiResponse<object>.Create(ResponseStatus.UserSoftDeleteSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("My-Menus")]
        public async Task<IActionResult> GetMyMenus()
        {
            var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var roles = HttpContext.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            var menus = await _service.GetMenusFromUserAsync(userId, roles);

            return Ok(ApiResponse<List<MenuDTO>>.SuccessResponse(menus, "Menus retrieved successfully"));
        }
    }
}
