using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IRepository
{
    public interface ILeaveRepository : IRepository<Leave>
    {
        Task<List<Leave>> GetLeaveByUserAAsync(int id, string status, int pageNumber, int pageSize);
        Task<List<Leave>> GetLeaveByUserDAsync(int id, string status, int pageNumber, int pageSize);
        Task<List<Leave>> GetLeavePendingAsync();
        Task<Leave> GetLeaveByIdStartDateAsync(int id, DateOnly StartDate);
        Task<List<Employee>> GetEmployeesOnLeaveAsync(DateOnly date);
        Task<int> LeaveTakenAsync(int id, int year);
    }
}
