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
    public class SubjectController : Controller
    {
        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var items = await _service.GetAllAsync();
            var response = ApiResponse<IEnumerable<SubjectDTO>>.Create(ResponseStatus.SubjectRetriveSuccessfully, items);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(ApiResponse<object>.Create(ResponseStatus.SubjectNotFound));
            return Ok(ApiResponse<SubjectDTO>.Create(ResponseStatus.SubjectRetriveSuccessfully, item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubjectDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            if (created == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to add subject (Invalid Course)"));
            var response = ApiResponse<object>.Create(ResponseStatus.SubjectAddedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectDTO dto)
        {
            var res = await _service.UpdateAsync(id, dto);
            if (res == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to update subject"));
            var response = ApiResponse<object>.Create(ResponseStatus.SubjectUpdatedSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var res = await _service.DeleteAsync(id);
            if (!res) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to delete subject"));
            var response = ApiResponse<object>.Create(ResponseStatus.SubjectSoftDeletedSuccessfully);
            return StatusCode(response.StatusCodes, response);
        }
    }
}
