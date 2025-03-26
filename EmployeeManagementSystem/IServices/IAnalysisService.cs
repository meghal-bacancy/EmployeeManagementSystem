using EmployeeManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.IServices
{
    public interface IAnalysisService
    {
        Task<TotalTimeLoggedDTO?> TotalLoggedHours(int id, DateOnly date, string duration);
        Task<byte[]> ExportTimesheetsToExcelAsync(int id, char order = 'A', int pageNumber = 1, int pageSize = 10);
    }
}
