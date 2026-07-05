using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoutePermissionsController : Controller
    {
        private readonly IRoutePermissionService _service;

        public RoutePermissionsController(IRoutePermissionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var items = await _service.GetAllRoutePermissionsAsync();
            var response = ApiResponse<IEnumerable<RoutePermissionDTO>>.SuccessResponse(items, "Route Permissions retrieved successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetRoutePermissionByIdAsync(id);
            if (item == null) return NotFound(ApiResponse<object>.Create(ResponseStatus.PermissionNotFound));
            return Ok(ApiResponse<RoutePermissionDTO>.SuccessResponse(item, "Route Permission retrieved successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoutePermissionDTO dto)
        {
            var created = await _service.CreateRoutePermissionAsync(dto);
            var response = ApiResponse<object>.SuccessResponse(created, "Route Permission Added Successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoutePermissionDTO dto)
        {
            var res = await _service.UpdateRoutePermissionAsync(id, dto);
            if (!res) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to update route permission"));
            var response = ApiResponse<object>.SuccessResponse(null, "Route Permission Updated Successfully");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _service.DeleteRoutePermissionAsync(id);
            if (!res) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete route permission"));
            var response = ApiResponse<object>.SuccessResponse(null, "Route Permission Deleted Successfully");
            return StatusCode(response.StatusCodes, response);
        }
    }
}
