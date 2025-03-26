using EmployeeManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.IServices;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IEmployeeServices _employeeServices;
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices, IEmployeeServices employeeServices, IAdminService adminService)
        {
            _adminService = adminService;
            _employeeServices = employeeServices;
            _authServices = authServices;
        }

        private int? GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return idClaim != null && int.TryParse(idClaim, out int userId) ? userId : (int?)null;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            var emp = await _employeeServices.GetEmployeeByIDAsyncIsActive(loginDTO.Email.ToLower());
            if (emp != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, emp.Password))
            {
                return Ok(new { Token = _authServices.GenerateToken(emp.EmployeeID, "Employee") });
            }

            var admin = await _adminService.GetAdminByIDAsyncIsActive(loginDTO.Email.ToLower());
            if (admin != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, admin.Password))
            {
                return Ok(new { Token = _authServices.GenerateToken(admin.AdminID, "Admin") });
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }

        [HttpPost("resetPassword")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> resetPassword(ResetPasswordDTO resetPasswordDTO)
        {

            return Ok("Password Updated Succesfully");
        }

        [HttpPost("resetPassword")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> resetord(ResetPasswordDTO resetPasswordDTO)
        {

            return Ok("Password Updated Succesfully");
        }
    }
}
