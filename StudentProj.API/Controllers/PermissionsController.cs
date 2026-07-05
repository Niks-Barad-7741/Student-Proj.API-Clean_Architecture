using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : Controller
    {
        private readonly IPermissionService _service;
        private readonly IMenuService _menuService;
        private readonly IRoleService _roleService;

        public PermissionsController(IPermissionService service, IMenuService menuService, IRoleService roleService)
        {
            _service = service;
            _menuService = menuService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var items = await _service.GetAllPermissionAsync();
            var response = ApiResponse<IEnumerable<PermissionDTO>>.Create(ResponseStatus.PermissionRetriveSuccessfully, items);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var perm = await _service.GetPermissionByIdAsync(id);
            if (perm == null) return NotFound(ApiResponse<object>.Create(ResponseStatus.PermissionNotFound));
            return Ok(ApiResponse<PermissionDTO>.Create(ResponseStatus.PermissionRetriveSuccessfully, perm));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDTO dto)
        {
            var exists = await _service.PermissionExistsAsync(dto.PermissionName);
            if (exists)
                return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "A permission with this name already exists"));

            var created = await _service.CreatePermissionAsync(dto);
            var response = ApiResponse<object>.Create(ResponseStatus.UserAddedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignPermission(AssignPermissionDTO dto)
        {
            var role = await _roleService.GetRoleByNameAsync(dto.RoleName);
            if (role == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Role not found"));

            var menu = await _menuService.GetMenuByNameAsync(dto.MenuName);
            if (menu == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Menu not found"));

            var pNames = dto.PermissionNames.Split(',');
            var notFound = new List<string>();
            foreach (var pName in pNames)
            {
                var perm = await _service.GetPermissionByNameAsync(pName.Trim());
                if (perm != null)
                    await _service.AssignPermissionToRoleAsync(role.Id, perm.Id, menu.Id);
                else
                    notFound.Add(pName.Trim());
            }

            if (notFound.Count == pNames.Length)
                return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, $"None of the permissions were found: {string.Join(", ", notFound)}"));

            var response = ApiResponse<object>.Create(ResponseStatus.PermissionAssignedSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionDTO dto)
        {
            var (success, error) = await _service.UpdatePermissionRoleAsync(id, dto);
            if (!success) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, error ?? "Failed to update permission"));
            var response = ApiResponse<object>.Create(ResponseStatus.UserUpdatedSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var res = await _service.DeletePermissionAsync(id);
            if (!res) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete permission"));
            var response = ApiResponse<object>.Create(ResponseStatus.UserSoftDeleteSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokePermission(AssignPermissionDTO dto)
        {
            var role = await _roleService.GetRoleByNameAsync(dto.RoleName);
            if (role == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Role not found"));

            var menu = await _menuService.GetMenuByNameAsync(dto.MenuName);
            if (menu == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Menu not found"));

            var pNames = dto.PermissionNames.Split(',');
            var notFound = new List<string>();
            foreach (var pName in pNames)
            {
                var perm = await _service.GetPermissionByNameAsync(pName.Trim());
                if (perm != null)
                    await _service.RemovePermissionFromRoleAsync(role.Id, perm.Id, menu.Id);
                else
                    notFound.Add(pName.Trim());
            }

            if (notFound.Count == pNames.Length)
                return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, $"None of the permissions were found: {string.Join(", ", notFound)}"));

            return Ok(ApiResponse<object>.SuccessResponse("Permission revoked successfully"));
        }
    }
}
