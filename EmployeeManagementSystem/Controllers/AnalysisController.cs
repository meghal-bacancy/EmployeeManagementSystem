using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpGet("employee/totalLoggedHours/{duration}/{date}")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> TotalLoggedHours([FromRoute] DateOnly date, [FromRoute] string duration)
        {
            int? userId = UserHelper.GetUserId(HttpContext);
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
        public async Task<IActionResult> ExportTimesheetsToExcelAsync([FromRoute] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] char order = 'A')
        {
            var fileContent = await _analysisService.ExportTimesheetsToExcel(id, order, pageNumber, pageSize);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Timesheets.xlsx");
        }

        [HttpGet("admin/leavesRemaining")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> LeavesRemainingAdmin([FromQuery] GetAnalyticsLeaveDTO getAnalyticsLeaveDTO)
        {
            var analyticsLeavesDTO = await _analysisService.LeavesRemaining(getAnalyticsLeaveDTO.id, getAnalyticsLeaveDTO.year);

            return Ok(new { analyticsLeavesDTO });
        }

        [HttpGet("admin/analyticsTime/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AnalyticsTime([FromRoute] int id, [FromQuery] GetAnalyticsTimeDTO getAnalyticsTimeDTO)
        {
            var analyticsTimeDTO = await _analysisService.TimeAnalytics(id, getAnalyticsTimeDTO.StarDate, getAnalyticsTimeDTO.EndDate);

            return Ok(new { analyticsTimeDTO });
        }
    }
}