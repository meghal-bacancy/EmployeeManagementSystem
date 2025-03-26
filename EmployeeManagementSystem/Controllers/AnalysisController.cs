using System.Security.Claims;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService, ITimesheetRepository timesheetRepository)
        {
            _analysisService = analysisService;
            _timesheetRepository = timesheetRepository;
        }

        private int? GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return idClaim != null && int.TryParse(idClaim, out int userId) ? userId : (int?)null;
        }

        [HttpGet("employee/totalLoggedHours/{duration}/{date}")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> TotalLoggedHours([FromRoute] DateOnly date, [FromRoute] string duration)
        {
            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            TotalTimeLoggedDTO? timeLoggedDTO = await _analysisService.TotalLoggedHours(userId.Value, date, duration);
            if (timeLoggedDTO == null)
                return BadRequest(new { Message = "Duration Invalid" });

            return Ok(new { Message = timeLoggedDTO });
        }

        [HttpGet("admin/totalLoggedHours/{id}/{duration}/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> TotalLoggedHoursAdmin([FromRoute] int id, [FromRoute] DateOnly date, [FromRoute] string duration)
        {
            TotalTimeLoggedDTO? timeLoggedDTO = await _analysisService.TotalLoggedHours(id, date, duration);
            if (timeLoggedDTO == null)
                return BadRequest(new { Message = "Duration Invalid" });

            return Ok(new { Message = timeLoggedDTO });
        }

        [HttpGet("admin/exportTimesheetsToExceL/{id}/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ExportTimesheetsToExcelAsync([FromRoute] int id, [FromQuery] char order = 'A', [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var fileContent = await _analysisService.ExportTimesheetsToExcelAsync(id, order, pageNumber, pageSize);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Timesheets.xlsx");
        }
    }
}