using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.IRepository
{
    public interface IEmployeeRepository: IRepository<Employee>
    {
        Task<Employee?> GetEmployeeByIDAsyncIsActive(int employeeId);
        Task<Employee?> GetEmployeeByIDAsyncIsActive(string email);
        Task<Employee?> GetEmployeeByIDAsync(int employeeId);
        //Task<decimal> GetTotalHoursForWeekAsync(int employeeId, DateOnly referenceDate);
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}
