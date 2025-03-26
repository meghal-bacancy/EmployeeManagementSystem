using EmployeeManagementSystem.DTOs;

namespace EmployeeManagementSystem.IServices
{
    public interface IAuthServices
    {
        string GenerateToken(int ID, string role);
        Task<string> ResetPassword(string userRole, int id, ResetPasswordDTO resetPasswordDTO);
    }
}
