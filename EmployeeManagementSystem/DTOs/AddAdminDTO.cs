using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class AddAdminDTO
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
