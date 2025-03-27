using EmployeeManagementSystem.DTOs;
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

            try
            {
                Admin admin = await _adminService.AddAdminAsync(addAdminDTO);
                return CreatedAtAction(nameof(AddAdmin), new { id = admin.AdminID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal error occurred.",  ex.Message });
            }
        }

        [HttpPost("addDepartment")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentDTO addDepartmentDTO)
        {
            if (addDepartmentDTO == null || string.IsNullOrWhiteSpace(addDepartmentDTO.DepartmentName))
                return BadRequest("Department Name cannot be null or empty.");

            try
            {
                Department dep = await _adminService.AddDepartmentAsync(addDepartmentDTO);

                return CreatedAtAction(nameof(AddDepartment), new { ID = dep.DepartmentID }, dep);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPost("addEmployee")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO addEmployeeDTO)
        {
            if (addEmployeeDTO == null)
                return BadRequest("Data is null");

            Employee emp;

            try
            {
                emp = await _employeeServices.AddEmployee(addEmployeeDTO);

                if (emp == null)
                    return StatusCode(500, "Error occurred while adding the employee.");

                return CreatedAtAction(nameof(AddEmployee), new { id = emp.EmployeeID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }


        [HttpGet("viewAllEmployeeDetails")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> viewAllEmployeeDetails()
        {
            try
            {
                var employees = await _employeeServices.GetAllEmployees();

                if (employees == null || !employees.Any())
                    return NotFound("No employees found");

                return Ok(new { employees });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("employeeDetails/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> EmployeeDetails([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Employee ID");

            try
            {
                EmployeeDTO? emp = await _employeeServices.GetEmployeeDetails(id);

                if (emp == null)
                    return NotFound("Employee Not Found");

                return Ok(emp);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("updateEmployeeDetails/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> UpdateEmployeeDetails([FromRoute] int id, [FromBody] UpdateEmployeeDTO updateEmployeeDTO)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid employee ID." });

            if (updateEmployeeDTO == null)
                return BadRequest(new { Message = "Update data cannot be null." });

            try
            {
                bool isUpdated = await _employeeServices.UpdateEmployeeDetails(id, updateEmployeeDTO);

                if (!isUpdated)
                    return NotFound(new { Message = "Employee not found." });

                return Ok(new { Message = "Employee updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [HttpDelete("deactivateEmployee/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> DeactivateEmployee([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid employee ID." });

            try
            {
                bool isUpdated = await _employeeServices.DeactivateEmployee(id);

                if (!isUpdated)
                    return NotFound(new { Message = "Employee not found or already deactivated." });

                return Ok(new { Message = "Employee deactivated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }


        [HttpPut("activateEmployee/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> ActivateEmployee([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { Message = "Invalid employee ID." });

                bool IsUpdated = await _employeeServices.ActivateEmployee(id);
                if (IsUpdated)
                {
                    return Ok(new { Message = "Employee Activated Successfully." });
                }

                return NotFound(new { Message = "Employee not found or already Activated." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }
    }
}
