using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Enums;
using StudentProj.DTO;

namespace StudentProj.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var courses = await _service.GetAllAsync();
            var response = ApiResponse<IEnumerable<CourseDTO>>.Create(ResponseStatus.CourseRetriveSuccessfully, courses);
            return StatusCode(response.StatusCodes, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _service.GetByIdAsync(id);
            if (course == null)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.CourseNotFound);
                return StatusCode(bad.StatusCodes, bad);
            }
            var response = ApiResponse<object>.Create(ResponseStatus.CourseRetriveSuccessfully, course);
            return StatusCode(response.StatusCodes, response);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.InvalidData);
                return StatusCode(bad.StatusCodes, bad);
            }

            var created = await _service.CreateAsync(dto);
            var response = ApiResponse<object>.Create(ResponseStatus.CourseCreatedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.InvalidData);
                return StatusCode(bad.StatusCodes, bad);
            }

            var updated = await _service.UpdateAsync(id, dto);
            var response = ApiResponse<object>.Create(ResponseStatus.CourseUpdatedSuccessfully, updated);
            return StatusCode(response.StatusCodes, response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                var status = ApiResponse<object>.Create(ResponseStatus.CourseNotFound, deleted);
                return StatusCode(status.StatusCodes, status);
            }
            var response = ApiResponse<object>.Create(ResponseStatus.CourseSoftDeletedSuccessfully, deleted);
            return StatusCode(response.StatusCodes, response);
        }
        [HttpPost("{id}/subjects")]
        public async Task<IActionResult> AddSubject(int id, [FromBody] CreateSubjectDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var bad = ApiResponse<object>.Create(ResponseStatus.InvalidData);
                return StatusCode(bad.StatusCodes, bad);
            }
            var subject = await _service.AddSubjectAsync(id, dto);
            if (subject == null)
            {
                var status = ApiResponse<object>.Create(ResponseStatus.CourseNotFound);
                return StatusCode(status.StatusCodes, status);
            }
            var response = ApiResponse<object>.Create(ResponseStatus.SubjectAddedSuccessfully, subject);
            return StatusCode(response.StatusCodes, response);
        }
        [HttpGet("{id}/subjects")]
        public async Task<IActionResult> GetSubjects(int id)
        {
            var subjects = await _service.GetSubjectsAsync(id);
            var response = ApiResponse<IEnumerable<SubjectDTO>>.Create(ResponseStatus.SubjectRetriveSuccessfully, subjects);
            return StatusCode(response.StatusCodes, response);
        }
    }
}


