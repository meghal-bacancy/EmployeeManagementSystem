using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class GetAnalyticsLeaveDTO
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int year { get; set; }
    }
}
