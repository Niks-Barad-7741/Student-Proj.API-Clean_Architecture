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
    public class LogsController : ControllerBase
    {
        private readonly ILogsService _service;

        public LogsController(ILogsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL([FromQuery] string? email, [FromQuery] string? action)
        {
            var items = await _service.GetLogsAsync(new LogQueryDTO { Email = email, Action = action });
            var response = ApiResponse<IEnumerable<LogResponseDTO>>.Create(ResponseStatus.LogsRetriveSuccessfully, items);
            return StatusCode(response.StatusCodes, response);
        }
    }
}



