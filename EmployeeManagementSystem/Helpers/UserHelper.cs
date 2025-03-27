using System.Security.Claims;

namespace EmployeeManagementSystem.Helpers
{
    public class UserHelper
    {
        public static int? GetUserId(HttpContext httpContext)
        {
            var idClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return idClaim != null && int.TryParse(idClaim, out int userId) ? userId : (int?)null;
        }

        public static string? GetUserRole(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
