using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Admin?> GetAdminIDAsyncIsActive(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(e => e.Email == email && e.IsActive == true);
        }

        public async Task<Admin?> GetAdminIDAsyncIsActive(int id)
        {
            return await _context.Admins.FirstOrDefaultAsync(e => e.AdminID == id);
        }
    }
}
