namespace EmployeeManagementSystem.DTOs
{
    public class AnalyticsTimeDTO
    {
        public TimeOnly avgStartTime { get; set; }
        public TimeOnly avgEndTime { get; set; }
        public Decimal avgTotalHoursWorked { get; set; } 
    }
}
