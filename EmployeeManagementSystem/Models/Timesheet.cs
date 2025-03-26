namespace EmployeeManagementSystem.Models
{
    public class Timesheet
    {
        public int TimesheetID { get; set; }
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly ? EndTime { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
