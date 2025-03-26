namespace EmployeeManagementSystem.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly? DateofBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string ? Address { get; set; }
        public int  DepartmentID { get; set; }
        public string ? TechStack { get; set; }
    }
}
