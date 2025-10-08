using EmployeeAttendanceSystem.Domain;
using EmployeeAttendanceSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceSystem.Service
{
    public class AttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public AttendanceService(IAttendanceRepository attendanceRepository)
        { 
            _attendanceRepository = attendanceRepository;
        }
        private readonly TimeSpan StandardStartTime = new TimeSpan(8, 30, 0);
        private readonly TimeSpan StandardEndTime = new TimeSpan(17, 0, 0);
        private const decimal FixedBreakHours = 1.0M;
        private bool CalculateIsLate(DateTime clockIn)
        {
            DateTime standardStartTime = clockIn.Date.Add(StandardStartTime);
            return clockIn > standardStartTime;
        }
        private decimal? CalculateWorkHours(DateTime checkIn, DateTime checkOut)
        { 
            TimeSpan duration = checkOut - checkIn;
            decimal totalHours = (decimal)duration.TotalHours;
            decimal workHours = totalHours - FixedBreakHours;
            return Math.Max(0, workHours);
        }
        private bool CalculateIsEarlyLeave(DateTime clockOut)
        { 
            DateTime standardCheckOutTime = clockOut.Date.Add(StandardEndTime);
            return clockOut < standardCheckOutTime;
        }
        public void AddAttendanceRecord(int employeeId, DateTime clockTime)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(employeeId));
            }
            if (clockTime == default(DateTime))
            {
                throw new ArgumentException("打卡時間無效，不能為DateTime的預設值", nameof(clockTime));
            }

            // clockTime.Date 只能取得當天的日期，時間會被歸零
            DateTime todayDate = clockTime.Date;
            var existingRecord = _attendanceRepository.GetRecordByEmployeeIdAndDate(employeeId, todayDate);
            if (existingRecord == null)
            {
                var newRecord = new AttendanceRecord
                {
                    EmployeeId = employeeId,
                    Date = todayDate,
                    CheckIn = clockTime,
                    CheckOut = null,
                    WorkHours = null,
                    IsLate = CalculateIsLate(clockTime),
                    IsEarlyLeave = false,
                };
                _attendanceRepository.AddAttendanceRecord(newRecord);
            }
            else
            {
                if (existingRecord.CheckOut.HasValue)
                {
                    throw new InvalidOperationException("已經打過下班卡");
                }
                if (clockTime < existingRecord.CheckIn.Value)
                {
                    throw new InvalidOperationException("下班打卡時間不能比上班打卡時間還早");
                }
                existingRecord.CheckOut = clockTime;
                existingRecord.WorkHours = CalculateWorkHours(existingRecord.CheckIn.Value, clockTime);
                existingRecord.IsEarlyLeave = CalculateIsEarlyLeave(clockTime);
                _attendanceRepository.UpdateAttendanceRecord(existingRecord);
            }
         }
    }
}
