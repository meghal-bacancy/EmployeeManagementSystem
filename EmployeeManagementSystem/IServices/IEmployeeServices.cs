using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;


namespace EmployeeManagementSystem.IServices
{
    public interface IEmployeeServices
    {
        Task<Employee> AddEmployee(EmployeeDTO addEmployeeDTO);
        Task<List<EmployeeDTO>> GetAllEmployees();
        Task<EmployeeDTO?> GetEmployeeDetails(int userId);
        Task<bool> UpdateEmployeeDetails(int userId, UpdateEmployeeDTO updateEmployeeDTO);
        Task<bool> DeactivateEmployee(int id);
        Task<bool> ActivateEmployee(int id);
    }
}
