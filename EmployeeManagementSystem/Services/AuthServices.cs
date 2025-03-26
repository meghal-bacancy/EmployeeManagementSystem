using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystem.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _config;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly Dictionary<string, Func<int, ResetPasswordDTO, Task<string>>> _resetPasswordHandlers;


        public AuthServices(IConfiguration config, IEmployeeRepository employeeRepository, IAdminRepository adminRepository)
        {
            _config = config;
            _employeeRepository = employeeRepository;
            _adminRepository = adminRepository;

            _resetPasswordHandlers = new Dictionary<string, Func<int, ResetPasswordDTO, Task<string>>>
            {
                { "Employee", ResetEmployeePassword },
                { "Admin", ResetAdminPassword }
            };

        }

        public string GenerateToken(int ID, string role)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, ID.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            var emp = await _employeeRepository.GetEmployeeByIDAsyncIsActive(email.ToLower());
            if (emp != null && BCrypt.Net.BCrypt.Verify(password, emp.Password))
            {
                return GenerateToken(emp.EmployeeID, "Employee");
            }

            var admin = await _adminRepository.GetAdminIDAsyncIsActive(email.ToLower());
            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                return GenerateToken(admin.AdminID, "Admin");
            }

            return null;
        }

        public async Task<string> ResetPassword(string userRole, int id, ResetPasswordDTO resetPasswordDTO)
        {
            if (_resetPasswordHandlers.TryGetValue(userRole, out var handler))
            {
                return await handler(id, resetPasswordDTO);
            }
            return "Invalid Role";
        }

        private async Task<string> ResetEmployeePassword(int id, ResetPasswordDTO resetPasswordDTO)
        {
            var emp = await _employeeRepository.GetEmployeeByIDAsync(id);
            if (emp == null)
                return "Employee Not Found";

            if (!BCrypt.Net.BCrypt.Verify(resetPasswordDTO.OldPassword, emp.Password))
                return "Old Password is wrong";

            emp.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
            await _employeeRepository.UpdateAsync(emp);
            return "Password Update Successful";
        }

        private async Task<string> ResetAdminPassword(int id, ResetPasswordDTO resetPasswordDTO)
        {
            var admin = await _adminRepository.GetAdminIDAsyncIsActive(id);
            if (admin == null)
                return "Admin Not Found";

            if (!BCrypt.Net.BCrypt.Verify(resetPasswordDTO.OldPassword, admin.Password))
                return "Old Password is wrong";

            admin.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
            await _adminRepository.UpdateAsync(admin);
            return "Password Update Successful";
        }
    }
}
