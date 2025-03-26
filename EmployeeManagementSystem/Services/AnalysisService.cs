using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public AnalysisService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public async Task<TotalTimeLoggedDTO?> TotalLoggedHours(int id, DateOnly date, string duration)
        {
            decimal totalHours;

            if (duration == "week")
                totalHours = await _timesheetRepository.GetTotalHoursForWeekAsync(id, date);
            else if (duration == "month")
                totalHours = await _timesheetRepository.GetTotalHoursForMonthAsync(id, date);
            else
                return null;

            TotalTimeLoggedDTO timeLoggedDTO = new TotalTimeLoggedDTO
            {
                Duration = duration,
                TotalHoursLogged = totalHours,
                StartDate = date
            };
            return timeLoggedDTO;
        }

        public async Task<byte[]> ExportTimesheetsToExcelAsync(int id, char order = 'A', int pageNumber = 1, int pageSize = 10)
        {
            List<Timesheet> timesheets = order.ToString().ToUpper() == "A"
                ? await _timesheetRepository.GetTimesheetsByUserAAsync(id, pageNumber, pageSize)
                : await _timesheetRepository.GetTimesheetsByUserDAsync(id, pageNumber, pageSize);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Timesheets");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Employee ID";
            worksheet.Cell(1, 3).Value = "Date";
            worksheet.Cell(1, 4).Value = "Start Time";
            worksheet.Cell(1, 5).Value = "End Time";
            worksheet.Cell(1, 6).Value = "Hours Worked";
            worksheet.Cell(1, 7).Value = "Description";

            for (int i = 0; i < timesheets.Count; i++)
            {
                var row = i + 2;
                worksheet.Cell(row, 1).Value = timesheets[i].TimesheetID;
                worksheet.Cell(row, 2).Value = timesheets[i].EmployeeID;
                worksheet.Cell(row, 3).Value = timesheets[i].Date.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 4).Value = timesheets[i].StartTime.ToString("HH:mm");
                worksheet.Cell(row, 5).Value = timesheets[i].EndTime?.ToString("HH:mm") ?? "N/A";
                worksheet.Cell(row, 6).Value = timesheets[i].TotalHoursWorked;
                worksheet.Cell(row, 7).Value = timesheets[i].Description;
            }

            //using var stream = new MemoryStream();
            //workbook.SaveAs(stream);

            //var fileContent = stream.ToArray();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
