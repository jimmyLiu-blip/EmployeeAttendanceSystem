using EmployeeAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceSystem.Repository
{
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee employee);
        IEnumerable<Employee> GetAllEmployees();
        Dictionary<int, string> GetAllEmployeeNames();
        Employee GetEmployeeByEmployeeId(int employeeId);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int employeeId);
        
    }
}
