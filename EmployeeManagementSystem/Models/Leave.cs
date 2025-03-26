using System.Numerics;

namespace EmployeeManagementSystem.Models
{
    public class Leave
    {
        public int LeaveID { get; set; } //
        public int EmployeeID { get; set; } //
        public Employee Employee { get; set; }
        public DateOnly StartDate { get; set; } //
        public DateOnly EndDate { get; set; } //
        public string LeaveType { get; set; } //
        public string Reason { get; set; } // 
        public string Status { get; set; } //
        public DateTime AppliedAt { get; set; } //
        public static int TotalLeaves { get; set; } = 18;
    }
}
