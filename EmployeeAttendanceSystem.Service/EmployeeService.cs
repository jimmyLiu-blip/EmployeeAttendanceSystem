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
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var newEmployee = new Employee { Name = name, Department = department, JobTitle = jobTitle };

            _employeeRepository.AddEmployee(newEmployee);
        }
        public Dictionary<int, string> GetAllEmployeeNames()
        {
            var allEmployeeNames = _employeeRepository.GetAllEmployeeNames();
            if (!allEmployeeNames.Any())
            {
                throw new InvalidOperationException("目前無任何員工姓名存在");
            }
            return allEmployeeNames;
        }
        public IEnumerable <Employee> GetAllEmployees()
        {
            var allEmployees = _employeeRepository.GetAllEmployees();
            if (!allEmployees.Any())
            {
                throw new InvalidOperationException("目前無任何員工存在");
            }
                return allEmployees;
        }
        public Employee GetEmployeeByEmployeeId(int employeeId)
        {
            return _employeeRepository.GetEmployeeByEmployeeId(employeeId);
        }
        public void UpdateEmployee(int employeeId, string name, string department, string jobTitle)
        {
            var existingEmployee = _employeeRepository.GetEmployeeByEmployeeId(employeeId);
            if (existingEmployee == null)
            {
                throw new ArgumentException($"找不到EmployeeId為{employeeId}的員工資料，無法更新");
            }
            existingEmployee.Name = name;
            existingEmployee.Department = department;
            existingEmployee.JobTitle = jobTitle;
            _employeeRepository.UpdateEmployee(existingEmployee);
        }
        public void DeleteEmployee(int employeeId)
        {
            var existingEmployee = _employeeRepository.GetEmployeeByEmployeeId(employeeId);
            if (existingEmployee == null)
            {
                throw new ArgumentException($"找不到員工編號為{employeeId}的資料，無法刪除");
             }
            _employeeRepository.DeleteEmployee(employeeId);
        }
    }
}
