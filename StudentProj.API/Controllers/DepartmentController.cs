using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;

namespace StudentProj.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            var response = ApiResponse<IEnumerable<DepartmentDTO>>.SuccessResponse(departments, "Departments retrieved successfully.");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                var notFound = ApiResponse<object>.FailureResponse($"Department with Id {id} not found.", 404);
                return StatusCode(notFound.StatusCodes, notFound);
            }
            var response = ApiResponse<DepartmentDTO>.SuccessResponse(department, "Department retrieved successfully.");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var badRequest = ApiResponse<object>.FailureResponse("Invalid model state", 400);
                return StatusCode(badRequest.StatusCodes, badRequest);
            }

            var created = await _departmentService.CreateAsync(dto);
            var response = ApiResponse<DepartmentDTO>.SuccessResponse(created, "Department created successfully.");
            return StatusCode(201, response); // Explicit 201 Created
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var badRequest = ApiResponse<object>.FailureResponse("Invalid model state", 400);
                return StatusCode(badRequest.StatusCodes, badRequest);
            }

            try
            {
                var success = await _departmentService.UpdateAsync(id, dto);
                if (success)
                {
                    var response = ApiResponse<object>.SuccessResponse(true, $"Department with Id {id} updated successfully.");
                    return StatusCode(response.StatusCodes, response);
                }
                var updateFailed = ApiResponse<object>.FailureResponse("Update failed.", 400);
                return StatusCode(updateFailed.StatusCodes, updateFailed);
            }
            catch (Exception ex)
            {
                var exResponse = ApiResponse<object>.FailureResponse(ex.Message, 404);
                return StatusCode(exResponse.StatusCodes, exResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _departmentService.DeleteAsync(id);
                if (success)
                {
                    var response = ApiResponse<object>.SuccessResponse(true, $"Department with Id {id} soft deleted successfully.");
                    return StatusCode(response.StatusCodes, response);
                }
                var deleteFailed = ApiResponse<object>.FailureResponse("Delete failed.", 400);
                return StatusCode(deleteFailed.StatusCodes, deleteFailed);
            }
            catch (Exception ex)
            {
                var exResponse = ApiResponse<object>.FailureResponse(ex.Message, 404);
                return StatusCode(exResponse.StatusCodes, exResponse);
            }
        }
    }
}
