using EmployeeManagementSystem.DTOs;

namespace EmployeeManagementSystem.IServices
{
    public interface IAuthServices
    {
        string GenerateToken(int ID, string role);
        Task<string?> Authenticate(string email, string password);
        Task<string> ResetPassword(string userRole, int id, ResetPasswordDTO resetPasswordDTO);
        Task<bool> SendPasswordResetEmail(string email);
        Task<bool> ResetPassword(string token, string newPassword);
    }
}
