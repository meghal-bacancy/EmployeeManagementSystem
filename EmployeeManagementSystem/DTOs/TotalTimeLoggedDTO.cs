namespace EmployeeManagementSystem.DTOs
{
    public class TotalTimeLoggedDTO
    {
        public string Duration { get; set; }
        public DateOnly StartDate { get; set; }
        public decimal TotalHoursLogged { get; set; }
    }
}
