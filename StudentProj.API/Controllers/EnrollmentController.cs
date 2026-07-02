using StudentProj.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _service;

        public EnrollmentController(IEnrollmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EnrollStudentDTO dto)
        {
            var created = await _service.EnrollStudentAsync(dto);
            var response = ApiResponse<object>.Create(ResponseStatus.EnrollmentAddedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var items = await _service.GetStudentByIdAsync(studentId);
            var response = ApiResponse<IEnumerable<EnrollmentDTO>>.Create(ResponseStatus.EnrollmentRetriveSuccessfully, items);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}/grade")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] EnrollmentDTO dto)
        {
            var result = await _service.UpdateGradeAsync(id, dto);
            if (result == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to update grade"));
            var response = ApiResponse<object>.SuccessResponse(result, "Grade updated successfully");
            return StatusCode(response.StatusCodes, response);
        }
    }
}
