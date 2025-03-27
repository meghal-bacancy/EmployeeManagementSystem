using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class AnalyticsTimeDTO
    {
        [Required]
        public TimeOnly avgStartTime { get; set; }
        [Required]
        public TimeOnly avgEndTime { get; set; }
        [Required]
        public Decimal avgTotalHoursWorked { get; set; } 
    }
}
