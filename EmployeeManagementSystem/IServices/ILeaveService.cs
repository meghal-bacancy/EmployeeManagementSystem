using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.IServices
{
    public interface ILeaveService
    {
        Task<string> AddLeave(int id, AddLeaveDTO addLeaveDTO);
        Task<List<Leave>> GetLeaveByUserStatusAsync(int id, string status, char order, int pageNumber, int pageSize);
        Task<List<Leave>> GetLeavePendingAsync();
        Task<List<Employee>> GetEmployeesOnLeave(DateOnly date);
        Task<string> LeaveAction(int id, DateOnly StartDate, string Action);
    }
}
