﻿using DocumentFormat.OpenXml.Spreadsheet;
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

            if (string.IsNullOrWhiteSpace(duration))
                return BadRequest(new { Message = "Duration parameter is required." });

            try
            {
                TotalTimeLoggedDTO? timeLoggedDTO = await _analysisService.TotalLoggedHours(userId.Value, date, duration);
                if (timeLoggedDTO == null)
                    return BadRequest(new { Message = "Invalid duration or no logged hours found." });

                return Ok(timeLoggedDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [HttpGet("admin/totalLoggedHours/{id}/{duration}/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> TotalLoggedHoursAdmin([FromRoute] int id, [FromRoute] DateOnly date, [FromRoute] string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return BadRequest(new { Message = "Duration parameter is required." });

            try
            {
                TotalTimeLoggedDTO? timeLoggedDTO = await _analysisService.TotalLoggedHours(id, date, duration);
                if (timeLoggedDTO == null)
                    return BadRequest(new { Message = "Invalid duration or no logged hours found." });

                return Ok(timeLoggedDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [HttpGet("admin/exportTimesheetsToExceL/{id}/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ExportTimesheetsToExcelAsync([FromRoute] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] char order = 'A')
        {
            try
            {
                var fileContent = await _analysisService.ExportTimesheetsToExcel(id, order, pageNumber, pageSize);

                if (fileContent == null || fileContent.Length == 0)
                    return NotFound(new { Message = "No timesheet data available for export." });

                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Timesheets.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [HttpGet("admin/analyticsTime/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AnalyticsTime([FromRoute] int id, [FromQuery] GetAnalyticsTimeDTO getAnalyticsTimeDTO)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid employee ID." });

            if (getAnalyticsTimeDTO.StarDate > getAnalyticsTimeDTO.EndDate)
                return BadRequest(new { Message = "Start date cannot be after end date." });

            try
            {
                var analyticsTimeDTO = await _analysisService.TimeAnalytics(id, getAnalyticsTimeDTO.StarDate, getAnalyticsTimeDTO.EndDate);

                if (analyticsTimeDTO == null)
                    return NotFound(new { Message = "No timesheet data available for the given period." });

                return Ok(new { analyticsTimeDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [HttpGet("admin/leavesRemaining")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> LeavesRemainingAdmin([FromQuery] GetAnalyticsLeaveDTO getAnalyticsLeaveDTO)
        {
            try
            {
                var analyticsLeavesDTO = await _analysisService.LeavesRemaining(getAnalyticsLeaveDTO.id, getAnalyticsLeaveDTO.year);

                if (analyticsLeavesDTO == null)
                    return NotFound(new { Message = "Employee not found or no leave records available." });

                return Ok(new { analyticsLeavesDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }
    }
}