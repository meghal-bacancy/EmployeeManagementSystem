using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IServices
{
    public interface ITimesheetService
    {
        Task<Timesheet?> GetTimesheetByDateAsync(int userId, DateOnly date);
        Task<ViewTimesheetDTO?> ViewTimesheet(int id, DateOnly date);
        Task<ViewTimesheetDTO?> ViewTimesheet(int id, DateOnly date, Employee employee);
        Task<List<Timesheet>> ViewTimesheets(int id, string order, int pageNumber, int pageSize);
        Task<bool> StartTimerAsync(int userId);
        Task<bool> EndTimerAsync(int userId, string description);
        Task<string> AddTimesheetAsync(int userId, AddTimesheetDTO addTimesheetDTO);
        Task<string> UpdateTimesheetAsync(int userId, UpdateTimesheetDTO updateTimesheetDTO);
        Task<bool> DeleteTimesheetAsync(int userId, DateOnly date);
    }
}
