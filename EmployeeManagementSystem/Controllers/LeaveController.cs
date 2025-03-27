using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            int? userId = UserHelper.GetUserId(HttpContext);
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            string msg = await _leaveService.AddLeave(userId.Value, addLeaveDTO);
            return Ok(new { Message = "Leave has been applied" });
        }

        [HttpGet("employee/viewLeave/{leaveType}")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewLeave([FromRoute] string leaveType, [FromQuery] char order = 'A', [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            int? userId = UserHelper.GetUserId(HttpContext);
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            List<Leave> leavesList = await _leaveService.GetLeaveByUserStatusAsync(userId.Value, leaveType, order, pageNumber, pageSize);

            if (leavesList == null)
                return NotFound("No leaves found");

            return Ok(new { Message = leavesList });
        }

        [HttpGet("admin/viewPendingLeave/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ViewPendingLeave()
        {
            List<Leave> leavesList = await _leaveService.GetLeavePendingAsync();

            if (!leavesList.Any())
                return NotFound("No leaves found");

            return Ok(new { Message = leavesList });
        }

        [HttpPut("admin/leaveAction/")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ApproveLeave(LeaveActionDTO leaveActionDTO)
        {
            string msg = await _leaveService.LeaveAction(leaveActionDTO.id, leaveActionDTO.StartDate, leaveActionDTO.action);

            if (msg == "leave updated succesfully")
                return Ok(new { Message = msg });
            return BadRequest(new { Message = msg });
        }

        [HttpGet("admin/employeeOnLeave/{date}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> EmployeeOnLeave([FromRoute] DateOnly date)
        {
            List<Employee> employeeOnLeave = await _leaveService.GetEmployeesOnLeave(date);

            if (!employeeOnLeave.Any())
                return NotFound("No one leaves");

            return Ok(new { Message = employeeOnLeave });
        }
    }
}
