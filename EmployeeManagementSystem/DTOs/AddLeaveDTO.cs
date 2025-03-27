using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class AddLeaveDTO
    {
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string LeaveType { get; set; }
        public string Reason { get; set; }
    }
}
