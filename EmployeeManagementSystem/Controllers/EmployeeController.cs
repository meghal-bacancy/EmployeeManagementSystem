using System.Security.Claims;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeeController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }

        [HttpGet("employeeDetails")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> EmployeeDetails()
        {
            int? userId = UserHelper.GetUserId(HttpContext);
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            EmployeeDTO emp = await _employeeServices.GetEmployeeDetails(userId.Value);

            if (emp == null)
                return NotFound("Employee Not Found");

            return Ok(emp);
        }

        [HttpPut("updateEmployeeDetails")]
        [Authorize(Policy = "EmployeeOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> UpdateEmployeeDetails([FromBody] UpdateEmployeeDTO updateEmployeeDTO)
        {
            int? userId = UserHelper.GetUserId(HttpContext);
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            bool isUpdated = await _employeeServices.UpdateEmployeeDetails(userId.Value, updateEmployeeDTO);

            if (!isUpdated)
                return NotFound(new { Message = "Employee not found." });

            return Ok(new { Message = "Employee updated successfully." });
        }
    }
}
