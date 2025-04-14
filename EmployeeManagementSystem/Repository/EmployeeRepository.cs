using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Employee?> GetEmployeeByIDAsyncIsActive(int employeeId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employeeId && e.IsActive == true);
        }

        public async Task<Employee?> GetEmployeeByIDAsync(int employeeId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employeeId);
        }

        public async Task<Employee?> GetEmployeeByIDAsyncIsActive(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.IsActive == true);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)  // Assuming `IsActive` is used to filter active employees
                .ToListAsync();
        }
    }
}
