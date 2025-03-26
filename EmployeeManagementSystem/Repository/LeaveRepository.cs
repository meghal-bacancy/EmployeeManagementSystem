using DocumentFormat.OpenXml.Bibliography;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        public LeaveRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Leave>> GetLeaveByUserAAsync(int id, string status, int pageNumber, int pageSize)
        {
            return await _context.Leaves
                                  .Where(t => t.EmployeeID == id && t.LeaveType == status)
                                  .OrderBy(t => t.StartDate)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }

        public async Task<List<Leave>> GetLeaveByUserDAsync(int id, string status, int pageNumber, int pageSize)
        {
            return await _context.Leaves
                                  .Where(t => t.EmployeeID == id && t.LeaveType == status)
                                  .OrderByDescending(t => t.StartDate)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }

        public async Task<List<Leave>> GetLeavePendingAsync()
        {
            return await _context.Leaves
                                 .Where(l => l.Status == "Pending")
                                 .ToListAsync();
        }

        public async Task<Leave> GetLeaveByIdStartDateAsync(int id, DateOnly StartDate)
        {
            return await _context.Leaves.FirstOrDefaultAsync(e => e.EmployeeID == id && e.StartDate == StartDate);
        }

        public async Task<List<Employee>> GetEmployeesOnLeaveAsync(DateOnly date)
        {
            return await _context.Leaves
                                 .Where(l => l.StartDate <= date && l.EndDate >= date && l.Status == "Approved")
                                 .Select(l => l.Employee)
                                 .ToListAsync();
        }

        public async Task<int> LeaveTakenAsync(int id, int year)
        {
            var leaves = await _context.Leaves.Where(l => l.EmployeeID == id && l.StartDate.Year == year && l.Status == "Approved")
                                              .ToListAsync();

            var totalLeaves = leaves.Sum(l => (new DateTime(l.EndDate.Year, l.EndDate.Month, l.EndDate.Day)
                                           - new DateTime(l.StartDate.Year, l.StartDate.Month, l.StartDate.Day)).Days + 1);

            return totalLeaves;
        }

    }
}
