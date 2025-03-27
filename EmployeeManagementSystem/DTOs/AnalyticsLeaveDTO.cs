using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class AnalyticsLeaveDTO
    {
        [Required]
        public int LeaveTaken { get; set; }
        [Required]
        public int LeaveLeft { get; set; }
    }
}
