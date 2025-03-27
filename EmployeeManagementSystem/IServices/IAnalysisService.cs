using EmployeeManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.IServices
{
    public interface IAnalysisService
    {
        Task<TotalTimeLoggedDTO?> TotalLoggedHours(int id, DateOnly date, string duration);
        Task<byte[]> ExportTimesheetsToExcel(int id, char order = 'A', int pageNumber = 1, int pageSize = 10);
        Task<AnalyticsTimeDTO> TimeAnalytics(int id, DateOnly startDate, DateOnly endDate);
        Task<AnalyticsLeaveDTO> LeavesRemaining(int id, int year);
    }
}
