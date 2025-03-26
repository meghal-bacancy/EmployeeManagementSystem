namespace EmployeeManagementSystem.DTOs
{
    public class ViewTimesheetDTO
    {
        public int? EmployeeID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public decimal? TotalHoursWorked { get; set; }
        public string? Description { get; set; }
    }
}
