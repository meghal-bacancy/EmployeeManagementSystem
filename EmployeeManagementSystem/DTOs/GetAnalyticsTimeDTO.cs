using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class GetAnalyticsTimeDTO
    {
        [Required]
        public DateOnly StarDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
    }
}
