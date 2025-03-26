using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.IServices
{
    public interface ILeaveService
    {
        Task<string> AddLeave(int id, AddLeaveDTO addLeaveDTO);
        Task<List<Leave>> GetLeaveByUserStatusAsync(int id, string status, int pageNumber, int pageSize, char order);
        Task<List<Leave>> GetLeavePendingAsync();
        Task<string> LeaveAction(int id, DateOnly StartDate, string Action);
        Task<List<Employee>> GetEmployeesOnLeave(DateOnly date);
    }
}
