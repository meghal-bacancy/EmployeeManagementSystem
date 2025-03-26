namespace EmployeeManagementSystem.DTOs
{
    public class AddLeaveDTO
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
    }
}
