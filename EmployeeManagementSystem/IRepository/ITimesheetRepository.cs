using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IRepository
{
    public interface ITimesheetRepository : IRepository<Timesheet>
    {
        Task<Timesheet?> GetTimesheetByDateAsync(int employeeId, DateOnly date);
        Task<decimal> GetTotalHoursForWeekAsync(int employeeId, DateOnly referenceDate);
        Task<decimal> GetTotalHoursForMonthAsync(int employeeId, DateOnly referenceDate);
        Task<List<Timesheet>> GetTimesheetsByUserAAsync(int id, int pageNumber, int pageSize);
        Task<List<Timesheet>> GetTimesheetsByUserDAsync(int id, int pageNumber, int pageSize);
        Task<List<Timesheet>> GetTimesheetsByIdDateAsync(int id, DateOnly StartDate, DateOnly EndDate);
    }
}
