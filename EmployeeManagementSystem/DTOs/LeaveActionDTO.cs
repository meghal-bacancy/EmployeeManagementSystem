using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class LeaveActionDTO
    {
        [Required]
        public int id { get; set; }
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        [MaxLength(20)]
        public string action { get; set; }
    }
}
