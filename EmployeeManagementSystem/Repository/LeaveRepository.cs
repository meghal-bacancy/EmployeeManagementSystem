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

    }
}
