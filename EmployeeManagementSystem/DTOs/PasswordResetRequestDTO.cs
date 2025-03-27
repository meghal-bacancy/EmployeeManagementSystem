using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class PasswordResetRequestDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
