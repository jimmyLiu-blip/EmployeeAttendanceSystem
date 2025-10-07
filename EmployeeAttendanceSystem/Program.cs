using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeAttendanceSystem.Infrastructure;
using EmployeeAttendanceSystem.Repository;
using EmployeeAttendanceSystem.Domain;
using EmployeeAttendanceSystem.Service;
using System.ComponentModel;

namespace EmployeeAttendanceSystem
{
    public class Program
    {
        private static readonly IEmployeeRepository _employeeRepository = new SqlEmployeeRepository();

        private static readonly EmployeeService _employeeService = new EmployeeService(_employeeRepository);
        static void Main(string[] args)
        {
            Console.WriteLine("正在測試資料庫連線：");
            try
            {
                // 因為它是一個靜態類別(static)，不需要透過 new 來建立物件實例
                // 所有成員（屬性、方法）都要是 static
                // 可以直接用「類別名稱」呼叫方法，不用使用依賴注入
                // 像「設定」、「工具方法」、「常數」、「共用函式」這類不需要儲存狀態的東西，就適合用靜態類別。
                DatabasebConfig.TestConnection();
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
            Console.WriteLine("4.離開系統");
            Console.Write("請輸入選項(1-4): ");
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
    }
}
