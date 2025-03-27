using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        //public async Task<Timesheet?> GetTimesheetByDateAsync(int userId, DateOnly date)
        //{
        //    return await _timesheetRepository.GetTimesheetByDateAsync(userId, date);
        //}


        //public async Task<ViewTimesheetDTO?> ViewTimesheet(int id, DateOnly date, Employee employee)
        //{
        //    var timesheet = await _timesheetRepository.GetTimesheetByDateAsync(id, date);

        //    if (timesheet == null)
        //        return null;

        //    var timesheetDTO = new ViewTimesheetDTO
        //    {
        //        EmployeeID = employee.EmployeeID,
        //        FirstName = employee.FirstName,
        //        LastName = employee.LastName,
        //        Email = employee.Email,
        //        Date = timesheet.Date,
        //        StartTime = timesheet.StartTime,
        //        EndTime = (TimeOnly)timesheet.EndTime,
        //        TotalHoursWorked = timesheet.TotalHoursWorked,
        //        Description = timesheet.Description
        //    };

        //    return timesheetDTO;
        //}

        public async Task<string> AddTimesheetAsync(int userId, AddTimesheetDTO addTimesheetDTO)
        {
            var existingTimesheet = await _timesheetRepository.GetTimesheetByDateAsync(userId, addTimesheetDTO.Date);
            if (existingTimesheet != null)
                return "Time Sheet already exist";

            if (addTimesheetDTO.StartTime != null && addTimesheetDTO.EndTime != null && addTimesheetDTO.StartTime < addTimesheetDTO.EndTime)
            {
                Timesheet timesheet = new Timesheet
                {
                    EmployeeID = userId,
                    Date = addTimesheetDTO.Date,
                    StartTime = addTimesheetDTO.StartTime,
                    EndTime = addTimesheetDTO.EndTime,
                    TotalHoursWorked = (decimal)(addTimesheetDTO.EndTime - addTimesheetDTO.StartTime).TotalHours,
                    Description = addTimesheetDTO.Description
                };
                await _timesheetRepository.AddAsync(timesheet);
                return "Timesheet added";
            }
            return "End time is lower than Start time";
        }

        public async Task<bool> StartTimerAsync(int userId)
        {
            var existingTimesheet = await _timesheetRepository.GetTimesheetByDateAsync(userId, DateOnly.FromDateTime(DateTime.Today));
            if (existingTimesheet != null)
                return false;

            Timesheet timesheet = new Timesheet
            {
                EmployeeID = userId,
                Date = DateOnly.FromDateTime(DateTime.Today),
                StartTime = TimeOnly.FromDateTime(DateTime.Now)
            };

            await _timesheetRepository.AddAsync(timesheet);
            return true;
        }

        public async Task<ViewTimesheetDTO?> ViewTimesheet(int id, DateOnly date)
        {
            var timesheet = await _timesheetRepository.GetTimesheetByDateAsync(id, date);

            if (timesheet == null)
                return null;

            var timesheetDTO = new ViewTimesheetDTO
            {
                Date = timesheet.Date,
                StartTime = timesheet.StartTime,
                EndTime = (TimeOnly)timesheet.EndTime,
                TotalHoursWorked = timesheet.TotalHoursWorked,
                Description = timesheet.Description
            };

            return timesheetDTO;
        }

        public async Task<List<Timesheet>> ViewTimesheets(int id, string order, int pageNumber, int pageSize)
        {
            List<Timesheet> timesheets = order.ToUpper() == "A"
                    ? await _timesheetRepository.GetTimesheetsByUserAAsync(id, pageNumber, pageSize)
                    : await _timesheetRepository.GetTimesheetsByUserDAsync(id, pageNumber, pageSize);

            return timesheets;
        }

        public async Task<bool> EndTimerAsync(int userId, string description)
        {
            var timesheet = await _timesheetRepository.GetTimesheetByDateAsync(userId, DateOnly.FromDateTime(DateTime.Today));
            if (timesheet == null)
                return false;

            timesheet.EndTime = TimeOnly.FromDateTime(DateTime.Now);

            if (timesheet.StartTime < timesheet.EndTime)
            {
                TimeSpan duration = (TimeSpan)(timesheet.EndTime - timesheet.StartTime);
                timesheet.TotalHoursWorked = (decimal)duration.TotalHours;
                timesheet.Description = description;
                await _timesheetRepository.UpdateAsync(timesheet);
                return true;
            }
            return false;
        }

        public async Task<string> UpdateTimesheet(int userId, UpdateTimesheetDTO updateTimesheetDTO)
        {
            var timesheet = await _timesheetRepository.GetTimesheetByDateAsync(userId, updateTimesheetDTO.Date);
            if (timesheet == null)
                return "Timesheet does not exist";

            timesheet.StartTime = updateTimesheetDTO.StartTime ?? timesheet.StartTime;
            timesheet.EndTime = updateTimesheetDTO.EndTime ?? timesheet.EndTime;
            timesheet.Description = updateTimesheetDTO.Description ?? timesheet.Description;

            if (timesheet.StartTime != null && timesheet.EndTime != null)
            {
                if (timesheet.StartTime < timesheet.EndTime)
                {
                    timesheet.TotalHoursWorked = (decimal)(timesheet.EndTime.Value - timesheet.StartTime).TotalHours;
                    await _timesheetRepository.UpdateAsync(timesheet);
                    return "Timesheet updated";
                }
                return "End time must be later than Start time";
            }

            return "Missing StartTime or EndTime";
        }

        public async Task<bool> DeleteTimesheet(int userId, DateOnly date)
        {
            var timesheet = await _timesheetRepository.GetTimesheetByDateAsync(userId, date);
            if (timesheet == null)
                return false;

            await _timesheetRepository.RemoveAsync(timesheet);
            return true;
        }
    }
}
