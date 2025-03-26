using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IServices
{
    public interface IAdminService
    {
        Task<Admin?> GetAdminByIDAsyncIsActive(string email);
        Task<Admin?> GetAdminByIDAsyncIsActive(int id);
        Task<Admin> AddAdminAsync(AddAdminDTO addAdminDTO);
        Task<Department> AddDepartmentAsync(AddDepartmentDTO addDepartmentDTO);
    }
}
