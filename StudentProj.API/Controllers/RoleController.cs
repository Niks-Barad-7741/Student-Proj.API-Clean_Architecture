using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Enums;
using StudentProj.Core.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _service;
        private readonly IRegisterRepository _auth; 

        public RoleController(IRoleService service, IRegisterRepository auth)
        {
            _service = service;
            _auth = auth;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var items = await _service.GetAllRolesAsync();
            var response = ApiResponse<IEnumerable<RoleDTO>>.Create(ResponseStatus.RoleRetriveSuccessfully, items);
            return StatusCode(response.StatusCodes, response);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _service.GetRoleByIdAsync(id);
            if (role == null) return NotFound(ApiResponse<object>.Create(ResponseStatus.RoleNotFound));
            return Ok(ApiResponse<RoleDTO>.Create(ResponseStatus.RoleRetriveSuccessfully, role));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDTO dto)
        {
            var created = await _service.CreateRoleAsync(dto);
            var response = ApiResponse<object>.Create(ResponseStatus.RoleCreatedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _service.DeleteRoleAsync(id);
            if (!result) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete role"));
            return Ok(ApiResponse<object>.Create(ResponseStatus.RoleDeletedSuccessfully));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDTO dto)
        {
            var result = await _service.UpdateRoleAsync(id, dto);
            if (!result) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to update role"));
            return Ok(ApiResponse<object>.Create(ResponseStatus.RoleUpdatedSuccessfully));
        }

        [HttpPost("assign")]
        public async Task<ActionResult> AssignRole(AssignRoleDTO dto)
        {
            var student = await _auth.GetStudentByIdAsync(dto.StudentId);
            if (student == null)
                return NotFound();

            var roleIds = dto.RoleIds.Split(',');
            foreach (var rId in roleIds)
            {
                await _auth.UpdateStudentRoleAsync(dto.StudentId, int.Parse(rId));
            }

            var success = ApiResponse<object>.SuccessResponse("Roles assigned successfully.");
            return StatusCode(200, success);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRole(AssignRoleDTO dto)
        {
            var roleIds = dto.RoleIds.Split(',');
            foreach (var rId in roleIds)
            {
                await _auth.RevokeRoleAsync(dto.StudentId, int.Parse(rId));
            }
            return Ok(ApiResponse<object>.SuccessResponse("Role revoked successfully"));
        }

        [HttpGet("My-Roles")]
        public async Task<IActionResult> GetMyRoles()
        {
            var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var roles = await _service.GetUserRolesAsync(userId);
            return Ok(ApiResponse<List<string>>.SuccessResponse(roles));
        }
    }
}
