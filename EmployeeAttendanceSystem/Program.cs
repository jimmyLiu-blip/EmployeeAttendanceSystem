using EmployeeAttendanceSystem.Domain;
using EmployeeAttendanceSystem.Infrastructure;
using EmployeeAttendanceSystem.Repository;
using EmployeeAttendanceSystem.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmployeeAttendanceSystem
{
    public class Program
    {
        private static readonly IEmployeeRepository _employeeRepository = new SqlEmployeeRepository();

        private static readonly EmployeeService _employeeService = new EmployeeService(_employeeRepository);

        private static readonly IAttendanceRepository _attendanceRepository = new SqlAttendanceRepository();

        private static readonly AttendanceService _attendanceService = new AttendanceService(_attendanceRepository);
        static void Main(string[] args)
        {
            Console.WriteLine("正在測試資料庫連線：");
            try
            {
                // 因為它是一個靜態類別(static)，不需要透過 new 來建立物件實例
                // 所有成員（屬性、方法）都要是 static
                // 可以直接用「類別名稱」呼叫方法，不用使用依賴注入
                // 像「設定」、「工具方法」、「常數」、「共用函式」這類不需要儲存狀態的東西，就適合用靜態類別。
                DatabaseConfig.TestConnection();
                Console.WriteLine("資料庫連線成功");
                Console.WriteLine("按任意鍵離開...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine("應用程式因資料庫錯誤而中止");
                Console.ResetColor();
                Console.WriteLine("按任意鍵離開...");
                Console.ReadKey();
                return;
            }

            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("   ===歡迎使用員工出勤系統===   ");
                    Showmenu();
                    string choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            AddEmployee();
                            break;
                        case "2":
                            GetAllEmployeeNames();
                            break;
                        case "3":
                            GetAllEmployees();
                            break;
                        case "4":
                            GetEmployeeByEmployeeId();
                            break;
                        case "5":
                            UpdateEmployee();
                            break;
                        case "6":
                            DeleteEmployee();
                            break;
                        case "7":
                            AddAttendanceRecord();
                            break;
                        case "8":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("輸入錯誤，請按任意鍵離開");
                            break;
                    }
                    if (!exit)
                    {
                        Console.WriteLine("按任意鍵繼續...");
                        Console.ReadKey();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"出現操作錯誤{ex.Message}");
                    Console.WriteLine("按任意鍵回到目錄頁面");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"出現異常錯誤{ex.Message}");
                    Console.WriteLine("按任意鍵回到目錄頁面");
                    Console.ReadLine();
                }
            }
        }
        private static void Showmenu()
        {
            Console.WriteLine("1.新增員工資料");
            Console.WriteLine("2.列出所有員工姓名");
            Console.WriteLine("3.列出所有員工完整資料");
            Console.WriteLine("4.透過員工編號取得員工資訊");
            Console.WriteLine("5.更新員工資料");
            Console.WriteLine("6.刪除員工資料");
            Console.WriteLine("7.新增打卡紀錄");
            Console.WriteLine("8.離開系統");
            Console.Write("請輸入選項(1-8): ");
        }
        private static void AddEmployee()
        {
            Console.WriteLine("===新增員工資料===");

            Console.Write("請輸入員工姓名(必填): ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("員工姓名為必填，請重新輸入");
                return;
            }
            Console.Write("請輸入部門(選填): ");
            string department = Console.ReadLine();
            Console.Write("請輸入職稱(選填): ");
            string jobTitle = Console.ReadLine();
            _employeeService.AddEmployee(name, department, jobTitle);
            Console.WriteLine("員工資料新增成功");
        }
        private static void GetAllEmployeeNames()
        {
            Console.WriteLine("===所有員工姓名列表===");
            var employeeNames = _employeeService.GetAllEmployeeNames();
            if(employeeNames == null || employeeNames.Count == 0)
            {
                Console.WriteLine("目前沒有任何員工資料");
                return;
            }
            foreach (var employeeName in employeeNames)
            {
                Console.WriteLine($"員工編號: {employeeName.Key}, 員工姓名: {employeeName.Value}");
            }
        }
        private static void GetAllEmployees()
        {
            Console.WriteLine("===所有員工完整資料列表===");
            var employees = _employeeService.GetAllEmployees();
            if (employees == null || !employees.Any())
            {
                Console.WriteLine("目前沒有任何員工資料");
                return;
            }
            foreach (var employee in employees)
            {
                string departmentDisplay = string.IsNullOrWhiteSpace(employee.Department) ? "無" : employee.Department;
                string jobTitleDisplay = string.IsNullOrWhiteSpace(employee.JobTitle) ? "無" : employee.JobTitle;
                Console.WriteLine($"員工編號: {employee.EmployeeId,-4}, 員工姓名: {employee.Name,-5}, 部門: {employee.Department,-6}, 職稱: {employee.JobTitle,-10}");
            }
        }
        private static void GetEmployeeByEmployeeId()
        {
            Console.WriteLine("===透過員工編號取得員工資訊===");
            Console.Write("請輸入員工編號: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("員工編號輸入錯誤，請重新輸入");
                return;
            }
            var employee = _employeeService.GetEmployeeByEmployeeId(employeeId);
            if (employee == null)
            {
                Console.WriteLine("查無此員工資料");
                return;
            }
            string departmentDisplay = string.IsNullOrWhiteSpace(employee.Department) ? "無" : employee.Department;
            string jobTitleDisplay = string.IsNullOrWhiteSpace(employee.JobTitle) ? "無" : employee.JobTitle;
            Console.WriteLine($"員工編號: {employee.EmployeeId}, 員工姓名: {employee.Name}, 部門: {departmentDisplay}, 職稱: {jobTitleDisplay}");
        }
        private static void UpdateEmployee()
        {
            Console.WriteLine("===更新員工資料===");
            Console.Write("請輸入員工編號：");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("員工編號輸入錯誤，慶重新輸入");
                return;
            }
            Console.Write("請輸入員工姓名：");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("員工姓名為必填，請重新輸入");
                return;
            }
            Console.Write("請輸入部門(選填)：");
            string department = Console.ReadLine();
            Console.Write("請輸入職稱(選填)：");
            string jobTitle = Console.ReadLine();
            try
            {
                _employeeService.UpdateEmployee(employeeId, name, department, jobTitle);
                Console.WriteLine($"更新員工資料成功，員工編號: {employeeId}, 員工姓名: {name}, 部門: {department}, 職稱: {jobTitle}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"更新失敗: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"出現異常錯誤: {ex.Message}", ex);
            }
        }
        private static void DeleteEmployee()
        {
  
                Console.WriteLine("===刪除員工資料===");
                Console.Write("請輸入員工編號：");
                if (!int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    Console.WriteLine("員工編號輸入錯誤，請重新輸入");
                    return;
                }
            try
            {
                var employee = _employeeService.GetEmployeeByEmployeeId(employeeId);
                if (employee == null)
                {
                    Console.WriteLine($"查無此員工編號：{employeeId}的資料，無法刪除");
                    return;
                }
                _employeeService.DeleteEmployee(employeeId);
                Console.WriteLine($"刪除員工成功，，員工編號: {employeeId}, 員工姓名: {employee.Name}, 部門: {employee.Department}, 職稱: {employee.JobTitle}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"刪除失敗: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"出現異常錯誤{ex.Message}", ex);
            }
        }
        private static void AddAttendanceRecord()
        {
            Console.WriteLine("===新增打卡紀錄===");
            Console.Write("請輸入你的員工編號：");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine($"查詢不到此員工編號：{employeeId},無法新增打卡紀錄");
                return;
            }

            DateTime clockTime = DateTime.Now;

            try
            {
                _attendanceService.AddAttendanceRecord(employeeId, clockTime);
                Console.WriteLine($"員工編號：{employeeId}打卡成功，時間為：{clockTime:yyyy/MM/dd HH:mm:ss}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"操作失敗，{ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"出現異常錯誤，{ex.Message}",ex);
            }
        }
    }
}
