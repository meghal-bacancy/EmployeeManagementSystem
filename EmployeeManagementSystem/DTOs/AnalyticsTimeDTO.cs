namespace EmployeeManagementSystem.DTOs
{
    public class AnalyticsTimeDTO
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Decimal TotalHoursWorked { get; set; } 
    }
}
