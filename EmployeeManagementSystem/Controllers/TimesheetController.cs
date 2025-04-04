﻿using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
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

        [HttpPost("employee/startTimer")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> StartTimer()
        {
            try
            {
                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                bool timmerStarted = await _timesheetService.StartTimerAsync(userId.Value);

                if (timmerStarted)
                    return Ok("Timmer Started");
                return Conflict(new { message = "Timmer already started." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while starting.", Error = ex.Message });
            }
        }

        [HttpPut("employee/endTimer")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> EndTimer([FromBody] TimesheetDescriptionDTO timesheetDescriptionDTO)
        {
            try
            {
                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                bool timmerStarted = await _timesheetService.EndTimerAsync(userId.Value, timesheetDescriptionDTO.Description);

                if (timmerStarted)
                    return Ok("Timmer Stoped");
                return NotFound(new { message = "No active timesheet found for today." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred ending you timmer.", Error = ex.Message });
            }
        }

        [HttpPost("employee/addTimesheet")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddTimesheet(AddTimesheetDTO addTimesheetDTO)
        {
            try
            {
                if (addTimesheetDTO == null)
                    return BadRequest(new { Message = "Invalid timesheet data." });

                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                string timesheetAdded = await _timesheetService.AddTimesheet(userId.Value, addTimesheetDTO);

                if (timesheetAdded == "Timesheet added")
                    return Ok(new { Message = timesheetAdded });
                return BadRequest(new { Message = timesheetAdded });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding your timesheet.", Error = ex.Message });
            }
        }

        [HttpGet("employee/viewTimesheet/{date}")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheet([FromRoute] DateOnly date)
        {
            try
            {          
                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                var timeSheet = await _timesheetService.ViewTimesheet(userId.Value, date);

                if (timeSheet == null)
                    return NotFound(new { Message = "Timesheet does not exist." });

                return Ok(timeSheet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching your request.", Error = ex.Message });
            }
        }

        [HttpGet("employee/viewTimesheets/")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] char order = 'A')
        {
            try
            {
                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                List<Timesheet> timesheets = await _timesheetService.ViewTimesheets(userId.Value, order, pageNumber, pageSize);

                if (!timesheets.Any())
                    return NotFound(new { Message = "No timesheets found." });

                return Ok(timesheets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching your request.", Error = ex.Message });
            }
        }

        [HttpPut("employee/updateTimesheet")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> UpdateTimesheet(UpdateTimesheetDTO updateTimesheetDTO)
        {
            try
            {
                if (updateTimesheetDTO == null)
                    return BadRequest(new { Message = "Invalid timesheet data." });

                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                string updatedTimesheet = await _timesheetService.UpdateTimesheet(userId.Value, updateTimesheetDTO);

                if (updatedTimesheet == "Timesheet updated")
                    return Ok(new { Message = updatedTimesheet });

                return BadRequest(new { Message = updatedTimesheet });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating you timesheet.", Error = ex.Message });
            }
        }


        [HttpPost("admin/addTimesheet/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddTimesheetAdmin([FromRoute] int id, AddTimesheetDTO addTimesheetDTO)
        {
            try
            {
                if (addTimesheetDTO == null)
                    return BadRequest(new { Message = "Invalid data." });

                string addedTimesheet = await _timesheetService.AddTimesheet(id, addTimesheetDTO);

                if (addedTimesheet == "Timesheet added")
                    return Ok(new { Message = addedTimesheet });

                return BadRequest(new { Message = addedTimesheet });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the timesheet.", Error = ex.Message });
            }
        }

        [HttpGet("admin/viewTimesheet/{id}/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheetAdmin([FromRoute] int id, [FromRoute] DateOnly date)
        {
            try
            {
                var timeSheet = await _timesheetService.ViewTimesheet(id, date);

                if (timeSheet == null)
                    return NotFound(new { Message = "Timesheet does not exist." });

                return Ok(timeSheet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching your request.", Error = ex.Message });
            }
        }

        [HttpGet("admin/viewTimesheets/{id}/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewTimesheets([FromRoute] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] char order = 'A')
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                    return BadRequest(new { Message = "Invalid pagination parameters." });

                List<Timesheet> timesheets = await _timesheetService.ViewTimesheets(id, order, pageNumber, pageSize);

                if (!timesheets.Any())
                    return NotFound(new { Message = "No timesheets found." });

                return Ok(timesheets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching your request.", Error = ex.Message });
            }
        }

        [HttpPut("admin/updateTimesheet/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> UpdateTimesheetAdmin([FromRoute] int id, UpdateTimesheetDTO updateTimesheetDTO)
        {
            try
            {
                if (updateTimesheetDTO == null || updateTimesheetDTO.Date == null)
                    return BadRequest(new { Message = "Invalid timesheet data." });

                if (updateTimesheetDTO == null)
                    return BadRequest(new { Message = "Invalid timesheet data." });

                string updatedTimesheet = await _timesheetService.UpdateTimesheet(id, updateTimesheetDTO);

                if (updatedTimesheet == "Timesheet updated")
                    return Ok(new { Message = updatedTimesheet });

                return BadRequest(new { Message = updatedTimesheet });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the timesheet.", Error = ex.Message });
            }
        }

        [HttpDelete("admin/deleteTimesheet/{id}/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> DeleteTimesheet([FromRoute] int id, [FromRoute] DateOnly date)
        {
            try
            {
                var timesheet = await _timesheetService.DeleteTimesheet(id, date);

                if (timesheet)
                    return Ok("Timesheet deleted");
                return NotFound(new { Message = "Timesheet does not exist." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error deleting the timesheet.", Error = ex.Message });
            }
        }
    }
}
