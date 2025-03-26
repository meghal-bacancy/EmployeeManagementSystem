using System;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly? DateofBirth { get; set; } 
        public string PhoneNumber { get; set; }
        public string? Address { get; set; } 
        public int DepartmentID { get; set; } 
        public Department Department { get; set; }
        public string? TechStack { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public List<Timesheet> Timesheets { get; set; }
        public List<Leave> Leaves { get; set; }
    }
}
