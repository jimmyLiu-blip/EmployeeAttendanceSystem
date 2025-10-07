using EmployeeAttendanceSystem.Domain;
using EmployeeAttendanceSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceSystem.Service
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public void AddEmployee(string name, string department, string jobTitle)
        {
            var newEmployee = new Employee { Name = name, Department = department, JobTitle = jobTitle };

            _employeeRepository.AddEmployee(newEmployee);
        }
        public Dictionary<int, string> GetAllEmployeeNames()
        { 
            return _employeeRepository.GetAllEmployeeNames();
        }
        public Employee GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }
    }
}
