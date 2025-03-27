using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class AddDepartmentDTO
    {
        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; }
    }
}
