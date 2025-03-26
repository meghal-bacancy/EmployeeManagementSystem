using DocumentFormat.OpenXml.Spreadsheet;
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

        public async Task<Admin?> GetAdminByIDAsyncIsActive(string email)
        {
            return await _adminRepository.GetAdminIDAsyncIsActive(email);
        }

        public async Task<Admin?> GetAdminByIDAsyncIsActive(int id)
        {
            return await _adminRepository.GetAdminIDAsyncIsActive(id);
        }

        public async Task<Admin> AddAdminAsync(AddAdminDTO addAdminDTO)
        {
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

        public async Task<Department> AddDepartmentAsync(AddDepartmentDTO addDepartmentDTO)
        {
            var department = new Department
            {
                DepartmentName = addDepartmentDTO.DepartmentName
            };

            await _departmentRepository.AddAsync(department);
            return department;
        }
    }
}
