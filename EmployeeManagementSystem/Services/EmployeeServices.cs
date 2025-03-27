using System.Threading.Tasks;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService : IEmployeeServices
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> AddEmployee(EmployeeDTO addEmployeeDTO)
        {
            try
            {
                var emp = new Employee
                {
                    FirstName = addEmployeeDTO.FirstName,
                    LastName = addEmployeeDTO.LastName,
                    Email = addEmployeeDTO.Email.ToLower(),
                    Password = BCrypt.Net.BCrypt.HashPassword(addEmployeeDTO.Password),
                    DateofBirth = addEmployeeDTO.DateofBirth,
                    PhoneNumber = addEmployeeDTO.PhoneNumber,
                    Address = addEmployeeDTO.Address,
                    DepartmentID = (int)addEmployeeDTO.DepartmentID,
                    TechStack = addEmployeeDTO.TechStack,
                    IsActive = true
                };

                await _employeeRepository.AddAsync(emp);

                return emp;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding employee", ex);
            }
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();

                return employees.Select(e => new EmployeeDTO
                {
                    EmployeeID = e.EmployeeID,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    DateofBirth = e.DateofBirth,
                    PhoneNumber = e.PhoneNumber,
                    Address = e.Address,
                    DepartmentID = e.DepartmentID,
                    TechStack = e.TechStack
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding employee", ex);
            }
        }

        public async Task<EmployeeDTO?> GetEmployeeDetails(int userId)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIDAsyncIsActive(userId);
                if (employee == null)
                    return null;

                return new EmployeeDTO
                {
                    EmployeeID = employee.EmployeeID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    DateofBirth = employee.DateofBirth,
                    PhoneNumber = employee.PhoneNumber,
                    Address = employee.Address,
                    DepartmentID = employee.DepartmentID,
                    TechStack = employee.TechStack
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching employee details for ID {userId}", ex);
            }
        }

        public async Task<bool> UpdateEmployeeDetails(int userId, UpdateEmployeeDTO updateEmployeeDTO)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIDAsyncIsActive(userId);
                if (employee == null)
                    return false;

                if (updateEmployeeDTO.PhoneNumber != null)
                    employee.PhoneNumber = updateEmployeeDTO.PhoneNumber;

                if (updateEmployeeDTO.TechStack != null)
                    employee.TechStack = updateEmployeeDTO.TechStack;

                if (updateEmployeeDTO.Address != null)
                    employee.Address = updateEmployeeDTO.Address;

                await _employeeRepository.UpdateAsync(employee);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating employee ID {userId}", ex);
            }
        }

        public async Task<bool> DeactivateEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIDAsyncIsActive(id);

                if (employee == null || !employee.IsActive)
                    return false;

                employee.IsActive = false;
                await _employeeRepository.UpdateAsync(employee);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deactivating employee ID {id}", ex);
            }
        }

        public async Task<bool> ActivateEmployee(int id)
        {
            try 
            { 
                var employee = await _employeeRepository.GetEmployeeByIDAsync(id);

                if (employee == null || employee.IsActive)
                    return false;

                employee.IsActive = true;

                await _employeeRepository.UpdateAsync(employee);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deactivating employee ID {id}", ex);
            }
        }
    }
}
