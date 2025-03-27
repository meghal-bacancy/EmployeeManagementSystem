using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly Dictionary<string, Func<int, DateOnly, Task<decimal>>> _durationHandlers;


        public AnalysisService(ITimesheetRepository timesheetRepository, ILeaveRepository leaveRepository)
        {
            _timesheetRepository = timesheetRepository;
            _leaveRepository = leaveRepository;
            _durationHandlers = new Dictionary<string, Func<int, DateOnly, Task<decimal>>>
            {
                { "week", _timesheetRepository.GetTotalHoursForWeekAsync },
                { "month", _timesheetRepository.GetTotalHoursForMonthAsync }
            };
        }

        public async Task<TotalTimeLoggedDTO?> TotalLoggedHours(int id, DateOnly date, string duration)
        {
            if (_durationHandlers.TryGetValue(duration, out var handler))
            {
                decimal totalHours = await handler(id, date);

                return new TotalTimeLoggedDTO
                {
                    Duration = duration,
                    TotalHoursLogged = totalHours,
                    StartDate = date
                };
            }
            return null;
        }

        public async Task<byte[]> ExportTimesheetsToExcel(int id, char order, int pageNumber = 1, int pageSize = 10)
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

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<AnalyticsTimeDTO> TimeAnalytics(int id, DateOnly StarDate, DateOnly EndDate)
        {
            List<Timesheet> timeLogs = await _timesheetRepository.GetTimesheetsByIdDateAsync(id, StarDate, EndDate);
            if (!timeLogs.Any())
            {
                return new AnalyticsTimeDTO
                {
                    avgStartTime = new TimeOnly(0, 0),
                    avgEndTime = new TimeOnly(0, 0),
                    avgTotalHoursWorked = 0
                };
            }

            var avgStartTime = new TimeOnly((int)timeLogs.Average(t => t.StartTime.Hour), (int)timeLogs.Average(t => t.StartTime.Minute));
            var avgEndTime = new TimeOnly((int)timeLogs.Average(t => t.EndTime?.Hour), (int)timeLogs.Average(t => t.EndTime?.Minute));
            var avgTotalHoursWorked = timeLogs.Average(t => (t.EndTime - t.StartTime)?.TotalHours);

            return new AnalyticsTimeDTO
            {
                avgStartTime = avgStartTime,
                avgEndTime = avgEndTime,
                avgTotalHoursWorked = (decimal)avgTotalHoursWorked
            };
        }

        public async Task<AnalyticsLeaveDTO> LeavesRemaining(int id, int year)
        {
            int leavesTaken = await _leaveRepository.LeaveTakenAsync(id, year);
            int leavesRemaing = Leave.TotalLeaves - leavesTaken;

            return new AnalyticsLeaveDTO
            {
                LeaveTaken = leavesTaken,
                LeaveLeft = leavesRemaing > 0 ? leavesRemaing : 0
            };
        }
    }
}
