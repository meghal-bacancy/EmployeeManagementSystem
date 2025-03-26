using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService, IEmployeeServices employeeServices)
        {
            _adminService = adminService;
            _employeeServices = employeeServices;
        }

        [HttpPost("addAdmin")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddAdmin([FromBody] AddAdminDTO addAdminDTO)
        {
            if (addAdminDTO == null)
                return BadRequest("Data is null");

            Admin admin = await _adminService.AddAdminAsync(addAdminDTO);

            return CreatedAtAction(nameof(AddAdmin), new { id = admin.AdminID });
        }

        [HttpPost("addDepartment")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentDTO addDepartmentDTO)
        {
            if (addDepartmentDTO == null)
                return BadRequest("Department Name is null");

            Department dep = await _adminService.AddDepartmentAsync(addDepartmentDTO);

            return CreatedAtAction(nameof(AddDepartment), new { ID = dep.DepartmentID });
        }

        [HttpPost("addEmployee")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO addEmployeeDTO)
        {
            if (addEmployeeDTO == null)
                return BadRequest("Data is null");

            Employee emp = await _employeeServices.AddEmployeeAsync(addEmployeeDTO);
            return CreatedAtAction(nameof(AddEmployee), new { id = emp.EmployeeID });
        }

        [HttpGet("employeeDetails/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> EmployeeDetails([FromRoute] int id)
        {
            EmployeeDTO emp = await _employeeServices.GetEmployeeDetailsAsync(id);

            if (emp == null)
                return NotFound("Employee Not Found");

            return Ok(emp);
        }

        [HttpPut("updateEmployeeDetails/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> UpdateEmployeeDetails([FromRoute] int id, [FromBody] UpdateEmployeeDTO updateEmployeeDTO)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid employee ID." });

            bool isUpdated = await _employeeServices.UpdateEmployeeDetailsAsync(id, updateEmployeeDTO);

            if (!isUpdated)
                return NotFound(new { Message = "Employee not found." });

            return Ok(new { Message = "Employee updated successfully." });
        }

        [HttpDelete("deactivateEmployee/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> DeactivateEmployee([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid employee ID." });

            bool IsUpdated = await _employeeServices.DeactivateEmployee(id);
            if (IsUpdated)
            {
                return Ok(new { Message = "Employee deactivated successfully." });
            }

            return NotFound(new { Message = "Employee Not Found." });
        }

        [HttpPut("activateEmployee/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ActivateEmployee([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid employee ID." });

            bool IsUpdated = await _employeeServices.ActivateEmployee(id);
            if (IsUpdated)
            {
                return Ok(new { Message = "Employee Activated Successfully." });
            }

            return NotFound(new { Message = "Employee Not Found." });
        }
    }
}
