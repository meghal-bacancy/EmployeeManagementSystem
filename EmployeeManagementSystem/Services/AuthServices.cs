using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystem.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _config;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;
        private const string ResetTokensKey = "PasswordResetTokens";
        private Dictionary<string, Func<int, ResetPasswordDTO, Task<string>>> _resetPasswordHandlers;
        private Dictionary<string, string> _passwordResetTokens = new Dictionary<string, string>();

        public AuthServices(IConfiguration config, IEmployeeRepository employeeRepository, IAdminRepository adminRepository, IEmailService emailService, IMemoryCache cache)
        {
            _config = config;
            _employeeRepository = employeeRepository;
            _adminRepository = adminRepository;
            _emailService = emailService;
            _cache = cache;

            _resetPasswordHandlers = new Dictionary<string, Func<int, ResetPasswordDTO, Task<string>>>
            {
                { "Employee", ResetEmployeePassword },
                { "Admin", ResetAdminPassword }
            };
        }

        public string GenerateToken(int ID, string role)
        {
            try
            {
                var keyString = _config["Jwt:Key"];
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];

                if (string.IsNullOrEmpty(keyString) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                    throw new InvalidOperationException("JWT configuration is missing required values.");

                var key = Encoding.UTF8.GetBytes(keyString);
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, ID.ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to generate JWT token.", ex);
            }
        }

        public async Task<string?> Authenticate(string email, string password)
        {
            try
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
            catch (ArgumentException argEx)
            {
                throw new ApplicationException($"Authentication failed: {argEx.Message}", argEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred during authentication.", ex);
            }
        }

        public async Task<string> ResetPassword(string userRole, int id, ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userRole))
                    throw new ArgumentException("User role cannot be empty.");

                if (!_resetPasswordHandlers.TryGetValue(userRole, out var handler))
                    return "Invalid Role";

                return await handler(id, resetPasswordDTO);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while resetting the password.", ex);
            }
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

        public async Task<bool> SendPasswordResetEmail(string email)
        {
            try
            {
                email = email.ToLower();
                var emp = await _employeeRepository.GetEmployeeByIDAsyncIsActive(email);
                var admin = await _adminRepository.GetAdminIDAsyncIsActive(email);

                if (emp == null && admin == null)
                {
                    return false;
                }

                string token = Guid.NewGuid().ToString();
                var tokens = _cache.Get<Dictionary<string, string>>(ResetTokensKey) ?? new Dictionary<string, string>();
                tokens[email] = token;
                _cache.Set(ResetTokensKey, tokens);

                string resetLink = $"/reset-password?token={token}";
                await _emailService.SendEmailAsync(email, "Password Reset Request", $"Token: {token}");

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to send password reset email.", ex);
            }
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            try
            {
                var tokens = _cache.Get<Dictionary<string, string>>(ResetTokensKey) ?? new Dictionary<string, string>();
                var email = tokens.FirstOrDefault(x => x.Value == token).Key;

                if (string.IsNullOrEmpty(email)) return false;

                var emp = await _employeeRepository.GetEmployeeByIDAsyncIsActive(email.ToLower());
                if (emp != null)
                {
                    emp.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    await _employeeRepository.UpdateAsync(emp);
                    tokens.Remove(email);
                    _cache.Set(ResetTokensKey, tokens);
                    return true;
                }

                var admin = await _adminRepository.GetAdminIDAsyncIsActive(email.ToLower());
                if (admin != null)
                {
                    admin.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    await _adminRepository.UpdateAsync(admin);
                    tokens.Remove(email);
                    _cache.Set(ResetTokensKey, tokens);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed reset password.", ex);
            }
        }
    }
}
