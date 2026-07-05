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
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _service;

        public AttendanceController(IAttendanceService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecordAttendanceDTO dto)
        {
            var created = await _service.RecordAsync(dto);
            if (created == null) return BadRequest(ApiResponse<object>.Create(ResponseStatus.BadRequest, "Failed to record attendance (Invalid Subject or Student)"));
            var response = ApiResponse<object>.Create(ResponseStatus.AttendanceAddedSuccessfully, created);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetBySubjectId(int subjectId, [FromQuery] DateTime? date)
        {
            var items = await _service.GetBySubjectIdAsync(subjectId, date);
            var response = ApiResponse<IEnumerable<AttendanceDTO>>.Create(ResponseStatus.AttendanceRetriveSuccessfully, items);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpGet("report/student/{studentId}")]
        public async Task<IActionResult> GetReportByStudent(int studentId)
        {
            var report = await _service.GetRecordAsync(studentId);
            var response = ApiResponse<object>.SuccessResponse(report, "Attendance report retrieved successfully");
            return StatusCode(response.StatusCodes, response);
        }
    }
}
