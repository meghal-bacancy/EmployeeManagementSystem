namespace EmployeeManagementSystem.DTOs
{
    public class UpdateTimesheetDTO
    {
        public DateOnly Date { get; set; }
        public TimeOnly ? StartTime { get; set; }
        public TimeOnly ? EndTime { get; set; }
        public string ? Description { get; set; }
    }
}
