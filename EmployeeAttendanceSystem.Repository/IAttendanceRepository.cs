using EmployeeAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceSystem.Repository
{
    public interface IAttendanceRepository
    {
        void AddAttendanceRecord(AttendanceRecord record);
        void UpdateAttendanceRecord(AttendanceRecord record);
        AttendanceRecord GetRecordByEmployeeIdAndDate(int employeeId, DateTime date);
        IEnumerable<AttendanceRecord> GetAllEmployeesTodayRecords();
        IEnumerable<AttendanceRecord> GetRecordsByEmployeeId(int employeeId);
    }
}
