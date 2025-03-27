using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IServices
{
    public interface IAdminService
    {
        Task<Admin> AddAdminAsync(AddAdminDTO addAdminDTO);
        Task<Department> AddDepartmentAsync(AddDepartmentDTO addDepartmentDTO);
    }
}
