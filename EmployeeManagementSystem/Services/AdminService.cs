using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public AdminService(IAdminRepository adminRepository, IDepartmentRepository departmentRepository)
        {
            _adminRepository = adminRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<Admin> AddAdminAsync(AddAdminDTO addAdminDTO)
        {
            try
            {
                var existingAdmin = await _adminRepository.GetAdminIDAsyncIsActive(addAdminDTO.Email);
                if (existingAdmin != null)
                {
                    throw new InvalidOperationException("An admin with this email already exists.");
                }

                var admin = new Admin
                {
                    FirstName = addAdminDTO.FirstName,
                    LastName = addAdminDTO.LastName,
                    Email = addAdminDTO.Email.ToLower(),
                    Password = BCrypt.Net.BCrypt.HashPassword(addAdminDTO.Password),
                    PhoneNumber = addAdminDTO.PhoneNumber,
                    IsActive = true
                };

                await _adminRepository.AddAsync(admin);
                return admin;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the admin.", ex);
            }
        }

        public async Task<Department> AddDepartmentAsync(AddDepartmentDTO addDepartmentDTO)
        {
            try
            {
                var department = new Department
                {
                    DepartmentName = addDepartmentDTO.DepartmentName
                };

                await _departmentRepository.AddAsync(department);
                return department;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding department", ex);
            }
        }
    }
}
