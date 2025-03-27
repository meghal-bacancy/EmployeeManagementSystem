using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IServices
{
    public interface ITimesheetService
    {
        Task<string> AddTimesheet(int userId, AddTimesheetDTO addTimesheetDTO);
        Task<bool> StartTimerAsync(int userId);
        Task<ViewTimesheetDTO?> ViewTimesheet(int id, DateOnly date);
        Task<List<Timesheet>> ViewTimesheets(int id, char order, int pageNumber, int pageSize);
        Task<bool> EndTimerAsync(int userId, string description);
        Task<string> UpdateTimesheet(int userId, UpdateTimesheetDTO updateTimesheetDTO);
        Task<bool> DeleteTimesheet(int userId, DateOnly date);
    }
}
