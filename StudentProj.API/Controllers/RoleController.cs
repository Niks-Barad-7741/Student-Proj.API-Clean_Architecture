using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Enums;
using StudentProj.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : ControllerBase
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
            var response = ApiResponse<RoleDTO>.Create(ResponseStatus.RoleRetriveSuccessfully, role);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDTO dto)
        {
            var exists = await _service.RoleExistsAsync(dto.RoleName);
            if (exists)
                return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "A role with this name already exists"));

            var created = await _service.CreateRoleAsync(dto);
            var response = ApiResponse<object>.Create(ResponseStatus.RoleCreatedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _service.DeleteRoleAsync(id);
            if (!result) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete role"));
            var response = ApiResponse<object>.Create(ResponseStatus.RoleDeletedSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDTO dto)
        {
            var (success, error) = await _service.UpdateRoleAsync(id, dto);
            if (!success) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, error ?? "Failed to update role"));
            var response = ApiResponse<object>.Create(ResponseStatus.RoleUpdatedSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost("assign")]
        public async Task<ActionResult> AssignRole(AssignRoleDTO dto)
        {
            var student = await _auth.GetStudentByIdAsync(dto.StudentId);
            if (student == null)
                return NotFound(ApiResponse<object>.Create(ResponseStatus.UserNotFound, "Student not found"));

            var roleNames = dto.RoleNames.Split(',');
            foreach (var rName in roleNames)
            {
                var role = await _service.GetRoleByNameAsync(rName.Trim());
                if (role != null)
                {
                    await _auth.UpdateStudentRoleAsync(dto.StudentId, role.Id);
                }
            }

            var success = ApiResponse<object>.SuccessResponse("Roles assigned successfully.");
            return StatusCode(200, success);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRole(AssignRoleDTO dto)
        {
            var student = await _auth.GetStudentByIdAsync(dto.StudentId);
            if (student == null)
                return NotFound(ApiResponse<object>.Create(ResponseStatus.UserNotFound, "Student not found"));

            var roleNames = dto.RoleNames.Split(',');
            foreach (var rName in roleNames)
            {
                var role = await _service.GetRoleByNameAsync(rName.Trim());
                if (role != null)
                {
                    await _auth.RevokeRoleAsync(dto.StudentId, role.Id);
                }
            }
            var response = ApiResponse<object>.Create(ResponseStatus.UserUpdatedSuccessfully, "Role revoked successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("My-Roles")]
        public async Task<IActionResult> GetMyRoles()
        {
            var userIdStr = HttpContext.User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var roles = await _service.GetUserRolesAsync(userId);
            var response = ApiResponse<List<string>>.Create(ResponseStatus.RoleRetriveSuccessfully, roles);
            return StatusCode(response.StatusCodes, response);
        }
    }
}
