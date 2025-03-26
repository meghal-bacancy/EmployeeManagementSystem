using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.IServices
{
    public interface IEmployeeServices
    {
        Task<Employee> AddEmployeeAsync(EmployeeDTO addEmployeeDTO);
        Task<EmployeeDTO?> GetEmployeeDetailsAsync(int userId);
        Task<Employee?> GetEmployeeByIDAsyncIsActive(int id);
        Task<Employee?> GetEmployeeByIDAsyncIsActive(string email);
        Task<bool> UpdateEmployeeDetailsAsync(int userId, UpdateEmployeeDTO updateEmployeeDTO);
        Task<bool> DeactivateEmployee(int id);
        Task<bool> ActivateEmployee(int id);

    }
}
