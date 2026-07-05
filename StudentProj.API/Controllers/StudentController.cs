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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var students = await _service.GetAllStudentsasync();
            var response = ApiResponse<IEnumerable<StudentDTO>>.Create(ResponseStatus.UserRetriveSuccessfully, students);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetStudentbyid(id);
            if (student == null)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.UserNotFound);
                return StatusCode(bad.StatusCodes, bad);
            }
            var response = ApiResponse<object>.Create(ResponseStatus.UserRetriveSuccessfully, student);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] RegisterDTO dto)
        {
            var newId = await _service.Createstudentasync(dto);
            if (newId <= 0)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Could not create student");
                return StatusCode(bad.StatusCodes, bad);
            }
            var response = ApiResponse<object>.Create(ResponseStatus.UserAddedSuccessfully, newId);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var student = await _service.Getstudentbynameasync(name);
            if (student == null)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.UserNotFound);
                return StatusCode(bad.StatusCodes, bad);
            }
            var response = ApiResponse<object>.Create(ResponseStatus.UserRetriveSuccessfully, student);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDTO dto)
        {
            var (success, error) = await _service.UpdateStudentasync(id, dto);
            if (!success)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.BadRequest, error ?? "Failed to update student");
                return StatusCode(bad.StatusCodes, bad);
            }
            var response = ApiResponse<object>.SuccessResponse(true, "Student updated successfully.");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentId(int id)
        {
            var result = await _service.DeleteStudentasync(id);
            if (!result)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete student");
                return StatusCode(bad.StatusCodes, bad);
            }
            var response = ApiResponse<object>.Create(ResponseStatus.UserSoftDeleteSuccessfully, true);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("upsert/{id?}")]
        public async Task<IActionResult> UpsertStudent(int? id, [FromBody] StudentDTO dto)
        {
            if (id.HasValue && id.Value > 0)
            {
                dto.Id = id.Value;
            }
            var newId = await _service.UpsertStudentAsync(dto);
            var response = ApiResponse<object>.SuccessResponse(newId, "Student upserted successfully");
            return StatusCode(response.StatusCodes, response);
        }
    }
}
