using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
