namespace EmployeeManagementSystem.DTOs
{
    public class ViewTimesheetDTO
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public decimal? TotalHoursWorked { get; set; }
        public string? Description { get; set; }
    }
}
