using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IRepository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Task<Admin?> GetAdminIDAsyncIsActive(string email);
        Task<Admin?> GetAdminIDAsyncIsActive(int id);
    }
}
