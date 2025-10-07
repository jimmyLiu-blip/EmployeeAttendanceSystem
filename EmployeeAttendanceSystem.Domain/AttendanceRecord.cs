using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceSystem.Domain
{
    public class AttendanceRecord
    {
        public int RecordId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckIn {  get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal? WorkHours { get; set; }
        public bool IsLate { get; set; }
        public bool IsEarlyLeave { get; set; }

    }
}
