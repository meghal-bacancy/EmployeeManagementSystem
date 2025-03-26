using System.Security.Claims;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly ITimesheetService _timesheetService;

        public TimesheetController(IEmployeeServices employeeServices, ITimesheetService timesheetService)
        {
            _employeeServices = employeeServices;
            _timesheetService = timesheetService;
        }

        private int? GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return idClaim != null && int.TryParse(idClaim, out int userId) ? userId : (int?)null;
        }

        [HttpPost("employee/startTimer")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> StartTimer()
        {
            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            bool timmerStarted = await _timesheetService.StartTimerAsync(userId.Value);

            if (timmerStarted)
                return Ok("Timmer Started");
            return Conflict(new { message = "Timmer already started." });
        }

        [HttpPut("employee/endTimer")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> EndTimer([FromBody] TimesheetDescriptionDTO timesheetDescriptionDTO)
        {
            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            bool timmerStarted = await _timesheetService.EndTimerAsync(userId.Value, timesheetDescriptionDTO.Description);

            if (timmerStarted)
                return Ok("Timmer Stoped");
            return NotFound(new { message = "No active timesheet found for today." });
        }

        [HttpPost("employee/addTimesheet")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddTimesheet(AddTimesheetDTO addTimesheetDTO)
        {
            if (addTimesheetDTO == null)
                return BadRequest(new { Message = "Invalid timesheet data." });

            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            string timesheetAdded = await _timesheetService.AddTimesheetAsync(userId.Value, addTimesheetDTO);

            if (timesheetAdded == "Timesheet added")
                return Ok(new { Message = timesheetAdded });
            return BadRequest(new { Message = timesheetAdded });

        }

        [HttpGet("employee/viewTimesheet/{date}")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheet([FromRoute] DateOnly date)
        {
            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            var timeSheet = await _timesheetService.ViewTimesheet(userId.Value, date);

            if (timeSheet == null)
                return NotFound(new { Message = "Timesheet does not exist." });

            return Ok(timeSheet);
        }

        [HttpGet("admin/viewTimesheets/")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheets([FromQuery] string order = "A", [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            List<Timesheet> timesheets = await _timesheetService.ViewTimesheets(userId.Value, order, pageNumber, pageSize);

            if (!timesheets.Any())
                return NotFound(new { Message = "No timesheets found." });

            return Ok(timesheets);
        }

        [HttpPut("employee/updateTimesheet")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> UpdateTimesheet(UpdateTimesheetDTO updateTimesheetDTO)
        {
            if (updateTimesheetDTO == null)
                return BadRequest(new { Message = "Invalid timesheet data." });

            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            string updatedTimesheet = await _timesheetService.UpdateTimesheetAsync(userId.Value, updateTimesheetDTO);

            if (updatedTimesheet == "Timesheet updated")
                return Ok(new { Message = updatedTimesheet });

            return BadRequest(new { Message = updatedTimesheet });
        }


        [HttpPost("admin/addTimesheet/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddTimesheetAdmin([FromRoute] int id, AddTimesheetDTO addTimesheetDTO)
        {
            if (addTimesheetDTO == null)
                return BadRequest(new { Message = "Invalid data." });

            string addedTimesheet = await _timesheetService.AddTimesheetAsync(id, addTimesheetDTO);

            if (addedTimesheet == "Timesheet added")
                return Ok(new { Message = addedTimesheet });

            return BadRequest(new { Message = addedTimesheet });
        }

        [HttpGet("admin/viewTimesheet/{id}/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheetAdmin([FromRoute] int id, [FromRoute] DateOnly date)
        {
            var timeSheet = await _timesheetService.ViewTimesheet(id, date);

            if (timeSheet == null)
                return NotFound(new { Message = "Timesheet does not exist." });

            return Ok(timeSheet);
        }

        [HttpGet("admin/viewTimesheets/{id}/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheets([FromRoute] int id, [FromQuery] string order = "A", [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest(new { Message = "Invalid pagination parameters." });

            List<Timesheet> timesheets = await _timesheetService.ViewTimesheets(id, order, pageNumber, pageSize);

            if (!timesheets.Any())
                return NotFound(new { Message = "No timesheets found." });

            return Ok(timesheets);
        }

        [HttpPut("admin/updateTimesheet/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> UpdateTimesheetAdmin([FromRoute] int id, UpdateTimesheetDTO updateTimesheetDTO)
        {
            if (updateTimesheetDTO == null || updateTimesheetDTO.Date == null)
                return BadRequest(new { Message = "Invalid timesheet data." });

            if (updateTimesheetDTO == null)
                return BadRequest(new { Message = "Invalid timesheet data." });

            string updatedTimesheet = await _timesheetService.UpdateTimesheetAsync(id, updateTimesheetDTO);

            if (updatedTimesheet == "Timesheet updated")
                return Ok(new { Message = updatedTimesheet });

            return BadRequest(new { Message = updatedTimesheet });
        }

        [HttpDelete("admin/deleteTimesheet/{id}/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> DeleteTimesheet([FromRoute] int id, [FromRoute] DateOnly date)
        {
            var timesheet = await _timesheetService.DeleteTimesheetAsync(id, date);

            if (timesheet)
                return Ok("Timesheet deleted");
            return NotFound(new { Message = "Timesheet does not exist." });
        }
    }
}
