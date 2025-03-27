using EmployeeManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagementSystem.Helpers;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;            
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrWhiteSpace(loginDTO.Email) || string.IsNullOrWhiteSpace(loginDTO.Password))
                return BadRequest(new { message = "Email and password are required." });

            try
            {
                var token = await _authServices.Authenticate(loginDTO.Email, loginDTO.Password);
                if (token != null)
                {
                    return Ok(new { Token = token });
                }

                return Unauthorized(new { message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }

        [HttpPut("resetPassword")]
        [Authorize(Policy = "RequireValidID")]
        public async Task<IActionResult> resetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            string? userRole = UserHelper.GetUserRole(HttpContext);
            if (userRole == null)
                return Unauthorized(new { Message = "Invalid or missing user Role in token." });

            int? userId = UserHelper.GetUserId(HttpContext);
            if (userId == null)
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            try
            {
                string msg = await _authServices.ResetPassword(userRole, userId.Value, resetPasswordDTO);

                if (msg == "Password Update Succesfull")
                    return Ok(new { Message = msg });

                return BadRequest(new { Message = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

        [HttpPut("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDTO request)
        {
            try
            {
                bool result = await _authServices.SendPasswordResetEmail(request.Email);

                if (result)
                    return Ok(new { Message = "Password reset email sent successfully." });

                return NotFound(new { Message = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordTokenDTO request)
        {
            try
            {
                var result = await _authServices.ResetPassword(request.Token, request.NewPassword);
                if (result) return Ok(new { message = "Password updated successfully" });

                return BadRequest(new { message = "Invalid or expired token" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }
    }
}
