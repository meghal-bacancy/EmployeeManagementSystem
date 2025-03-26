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

        public AuthServices(IConfiguration config, IEmployeeRepository employeeRepository, IAdminRepository adminRepository)
        {
            _config = config;
            _employeeRepository = employeeRepository;
            _adminRepository = adminRepository;

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
        public async Task<string> ResetPassword(string userRole, int id, ResetPasswordDTO resetPasswordDTO)
        {
            if (userRole == "Employee")
            {
                var emp = await _employeeRepository.GetEmployeeByIDAsync(id);

                if (emp == null)
                    return "Employee Not Found";
                if (BCrypt.Net.BCrypt.Verify(resetPasswordDTO.OldPassword, emp.Password))
                {
                    emp.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
                    await _employeeRepository.UpdateAsync(emp);
                    return "Password Update Succesfull";
                }
                return "Old Password is wrong";
            }
            else if (userRole == "Admin")
            {
                var admin = await _adminRepository.GetAdminIDAsyncIsActive(id);

                if (admin == null)
                    return "Employee Not Found";
                if (BCrypt.Net.BCrypt.Verify(resetPasswordDTO.OldPassword, admin.Password))
                {
                    admin.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
                    await _adminRepository.UpdateAsync(admin);
                    return "Password Update Succesfull";
                }
                return "Old Password is wrong";
            }
            return "Invalid Role";
        }
    }
}
