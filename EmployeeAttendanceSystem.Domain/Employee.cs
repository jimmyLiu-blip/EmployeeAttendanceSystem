using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceSystem.Domain
{
    public class Employee
    {
        public int EmployeeId { get; internal set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public Employee() { }
    }
}
