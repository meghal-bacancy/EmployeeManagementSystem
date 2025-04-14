using DocumentFormat.OpenXml.Wordprocessing;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class TimesheetRepository : Repository<Timesheet>, ITimesheetRepository
    {
        public TimesheetRepository(AppDbContext context) : base(context) { }

        public async Task<Timesheet?> GetTimesheetByDateAsync(int employeeId, DateOnly date)
        {
            return await _context.Timesheets
                .FirstOrDefaultAsync(t => t.EmployeeID == employeeId && t.Date == date);
        }

        public async Task<decimal> GetTotalHoursForWeekAsync(int employeeId, DateOnly referenceDate)
        {
            var endOfWeek = referenceDate.AddDays(6);

            return await _context.Timesheets
                .Where(t => t.EmployeeID == employeeId && t.Date >= referenceDate && t.Date <= endOfWeek)
                .SumAsync(t => t.TotalHoursWorked);
        }

        public async Task<decimal> GetTotalHoursForMonthAsync(int employeeId, DateOnly referenceDate)
        {
            var endOfWeek = referenceDate.AddDays(30);

            return await _context.Timesheets
                .Where(t => t.EmployeeID == employeeId && t.Date >= referenceDate && t.Date <= endOfWeek)
                .SumAsync(t => t.TotalHoursWorked);
        }

        public async Task<List<Timesheet>> GetTimesheetsByUserDAsync(int id, int pageNumber, int pageSize)
        {
            return await _context.Timesheets
                                  .Where(t => t.EmployeeID == id)
                                  .OrderByDescending(t => t.Date)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }

        public async Task<List<Timesheet>> GetTimesheetsByUserAAsync(int id, int pageNumber, int pageSize)
        {
            return await _context.Timesheets
                                  .Where(t => t.EmployeeID == id)
                                  .OrderBy(t => t.Date)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }

        public async Task<List<Timesheet>> GetTimesheetsByIdDateAsync(int id, DateOnly StartDate, DateOnly EndDate)
        {
            return await _context.Timesheets
                            .Where(t => t.EmployeeID == id && t.Date >= StartDate && t.Date <= EndDate)
                            .ToListAsync();
        }
    }
}
