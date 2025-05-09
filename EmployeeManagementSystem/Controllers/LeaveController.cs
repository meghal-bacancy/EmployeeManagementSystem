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
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly AppDbContext _context;

        public LeaveController(ILeaveService leaveService, AppDbContext context)
        {
            _leaveService = leaveService;
            _context = context;
        }

        [HttpPost("employee/applyLeave/")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddLeave([FromBody] AddLeaveDTO addLeaveDTO)
        {
            try
            {
                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                string msg = await _leaveService.AddLeave(userId.Value, addLeaveDTO);
                return Ok(new { Message = "Leave has been applied" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding leave.", Error = ex.Message });
            }
        }

        [HttpGet("employee/viewLeave/{status}")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewLeave([FromRoute] string status, [FromQuery] char order = 'A', [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                int? userId = UserHelper.GetUserId(HttpContext);
                if (userId == null)
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });

                List<Leave> leavesList = await _leaveService.GetLeaveByUserStatusAsync(userId.Value, status, order, pageNumber, pageSize);

                if (leavesList == null)
                    return NotFound("No leaves found");

                return Ok(new { Message = leavesList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching leave.", Error = ex.Message });
            }
        }

        [HttpGet("admin/viewPendingLeave/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewPendingLeave()
        {
            try
            {
                List<Leave> leavesList = await _leaveService.GetLeavePendingAsync();

                if (!leavesList.Any())
                    return NotFound("No leaves found");

                return Ok(new { Message = leavesList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching leave.", Error = ex.Message });
            }
        }

        [HttpPut("admin/leaveAction/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ApproveLeave(LeaveActionDTO leaveActionDTO)
        {
            try
            {
                string msg = await _leaveService.LeaveAction(leaveActionDTO.id, leaveActionDTO.StartDate, leaveActionDTO.action);

                if (msg == "leave updated succesfully")
                    return Ok(new { Message = msg });
                return BadRequest(new { Message = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating leave.", Error = ex.Message });
            }
        }

        [HttpGet("admin/employeeOnLeave/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> EmployeeOnLeave([FromRoute] DateOnly date)
        {
            try
            {
                List<Employee> employeeOnLeave = await _leaveService.GetEmployeesOnLeave(date);

                if (!employeeOnLeave.Any())
                    return NotFound("No one leaves");

                return Ok(new { Message = employeeOnLeave });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }
    }
}
