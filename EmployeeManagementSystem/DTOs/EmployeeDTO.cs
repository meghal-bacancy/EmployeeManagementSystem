using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly? DateofBirth { get; set; }
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public string ? Address { get; set; }
        public int  DepartmentID { get; set; }
        public string ? TechStack { get; set; }
    }
}
