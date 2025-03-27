using EmployeeManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IEmployeeServices _employeeServices;
        private readonly IAuthServices _authServices;
        private readonly IEmailService _emailService;

        public AuthController(IAuthServices authServices, IEmployeeServices employeeServices, IAdminService adminService, IEmailService emailService)
        {
            _adminService = adminService;
            _employeeServices = employeeServices;
            _authServices = authServices;            
            _emailService = emailService;

        }

        private string? GetUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        private int? GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return idClaim != null && int.TryParse(idClaim, out int userId) ? userId : (int?)null;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            var token = await _authServices.AuthenticateAsync(loginDTO.Email, loginDTO.Password);

            if (token != null)
            {
                return Ok(new { Token = token });
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }

        [HttpPut("resetPassword")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> resetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            string? userRole = GetUserRole();
            if (userRole == null)
                return Unauthorized(new { Message = "Invalid or missing user Role in token." });

            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            string msg = await _authServices.ResetPassword(userRole, userId.Value, resetPasswordDTO);

            if (msg == "Password Update Succesfull")
                return Ok(new { Message = msg });

            return BadRequest(new { Message = msg }); 
        }

        [HttpPut("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDTO request)
        {
            var result = await _authServices.SendPasswordResetEmailAsync(request.Email);
            if (result) return Ok(new { message = "Password reset email sent" });

            return NotFound(new { message = "User not found" });
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordTokenDTO request)
        {
            var result = await _authServices.ResetPassword(request.Token, request.NewPassword);
            if (result) return Ok(new { message = "Password updated successfully" });

            return BadRequest(new { message = "Invalid or expired token" });
        }
    }
}
