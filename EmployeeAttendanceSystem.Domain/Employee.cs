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

        /*public static Employee CreatNew(string name, string department, string jobTitle)
        { 
            if(string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentNullException("員工姓名不得為空",nameof(name));
            return new Employee { Name = name, Department = department, JobTitle = jobTitle }
        }*/

    }
}
