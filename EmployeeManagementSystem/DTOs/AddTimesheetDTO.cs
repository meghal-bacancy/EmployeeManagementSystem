namespace EmployeeManagementSystem.DTOs
{
    public class AddTimesheetDTO
    {
        public int EmployeeID { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Description { get; set; }
    }
}
