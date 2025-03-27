using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.IRepository;
using EmployeeManagementSystem.IServices;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repository;

namespace EmployeeManagementSystem.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private static readonly HashSet<string> ValidLeaveTypes = new() { "Sick", "Casual", "Vacation", "Other" };
        private static readonly HashSet<string> ValidStatuses = new() { "Approved", "Rejected" };

        public LeaveService (ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<string> AddLeave(int id, AddLeaveDTO addLeaveDTO)
        {
            try
            {
                if (!ValidLeaveTypes.Contains(addLeaveDTO.LeaveType))
                    return "Invalid leave type.";

                if (addLeaveDTO.StartDate > addLeaveDTO.EndDate)
                    return "StartDate cannot be later than EndDate.";

                Leave leave = new Leave
                {
                    EmployeeID = id,
                    StartDate = addLeaveDTO.StartDate,
                    EndDate = addLeaveDTO.EndDate,
                    LeaveType = addLeaveDTO.LeaveType.ToString(),
                    Reason = addLeaveDTO.Reason
                };

                await _leaveRepository.AddAsync(leave);

                return "Leave added succesfully";
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error adding Leave", ex);
            }
        }

        public async Task<List<Leave>> GetLeaveByUserStatusAsync(int id, string status, char order, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (!ValidStatuses.Contains(status))
                    return null;

                List<Leave> leave = order.ToString().ToUpper() == "A"
                        ? await _leaveRepository.GetLeaveByUserAAsync(id, status, pageNumber, pageSize)
                        : await _leaveRepository.GetLeaveByUserDAsync(id, status, pageNumber, pageSize);

                if (!leave.Any())
                    return null;

                return leave;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error adding Leave", ex);
            }
        }

        public async Task<List<Leave>> GetLeavePendingAsync()
        {
            return await _leaveRepository.GetLeavePendingAsync();
        }

        public async Task<List<Employee>> GetEmployeesOnLeave(DateOnly date)
        {
            return await _leaveRepository.GetEmployeesOnLeaveAsync(date);
        }

        public async Task<string> LeaveAction(int id, DateOnly StartDate, string Action)
        {
            var leave = await _leaveRepository.GetLeaveByIdStartDateAsync(id, StartDate);

            if (leave == null)
                return "No Leave Request Exist";

            if (!ValidStatuses.Contains(Action))
                return "Invalid status type.";

            leave.Status = Action;

            await _leaveRepository.UpdateAsync(leave);

            return "leave updated succesfully";
        }
    }
}
