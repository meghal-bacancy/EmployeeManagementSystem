namespace EmployeeManagementSystem.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        //Task<bool> SendPasswordResetEmailAsync(string email);
        //Task<bool> ResetPassword(string token, string newPassword);
    }
}
