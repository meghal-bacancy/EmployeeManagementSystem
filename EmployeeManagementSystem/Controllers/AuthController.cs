﻿using EmployeeManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.IServices;
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
    }
}
