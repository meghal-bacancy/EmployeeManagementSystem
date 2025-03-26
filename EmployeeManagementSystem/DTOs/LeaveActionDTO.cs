namespace EmployeeManagementSystem.DTOs
{
    public class LeaveActionDTO
    {
        public int id { get; set; }
        public DateOnly StartDate { get; set; }
        public string action { get; set; }
    }
}
