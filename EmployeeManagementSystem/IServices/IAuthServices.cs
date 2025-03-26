using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IServices
{
    public interface IAuthServices
    {
        string GenerateToken(int ID, string role);
        bool UpdatePassword(ResetPasswordDTO resetPasswordDTO);
    }
}
