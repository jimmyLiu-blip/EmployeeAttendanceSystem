using EmployeeAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using EmployeeAttendanceSystem.Infrastructure;
using System.CodeDom;

namespace EmployeeAttendanceSystem.Repository
{
    public class SqlEmployeeRepository:IEmployeeRepository
    {
        private Employee MapToEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = (int)reader["EmployeeId"],
                Name = (string)reader["Name"],
                Department = reader["Department"] == DBNull.Value ? null : (string)reader["Department"],
                JobTitle = reader["JobTitle"] == DBNull.Value ? null : (string)reader["JobTitle"]
            };
        }
        public SqlEmployeeRepository() { }
        public void AddEmployee(Employee employee)
        {
            using (var cn = new SqlConnection(DatabasebConfig.ConnectionString))
            {
                var sql = @"INSERT INTO Employee (Name, Department, JobTitle) 
                            OUTPUT INSERTED.EmployeeId
                            VALUES (@Name, @Department,@Title)";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = employee.Name;
                    cmd.Parameters.Add("@Department", System.Data.SqlDbType.NVarChar, 50).Value = (object)employee.Department ?? DBNull.Value;
                    cmd.Parameters.Add("@JobTitle", System.Data.SqlDbType.NVarChar, 50).Value = (object)employee.JobTitle ?? DBNull.Value;
                    cn.Open();

                    var newId = cmd.ExecuteScalar();

                    if (newId != null && newId != DBNull.Value)
                    { 
                        employee.EmployeeId = (int)newId;
                    }
                }
            }
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var cn = new SqlConnection(DatabasebConfig.ConnectionString))
            {
                var sql = @"SELECT EmployeeId, Name, Department, JobTitle From Employee";
                using (var cmd = new SqlCommand(sql, cn))
                { 
                    cn.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(MapToEmployee(reader));
                        }
                    }
                }
            }
            return employees;
        }
        public Dictionary<int, string> GetAllEmployeeNames()
        {
            var names = new Dictionary<int, string>();
            using (var cn = new SqlConnection(DatabasebConfig.ConnectionString))
            {
                var sql = @"SELECT EmployeeId, Name From Employee";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add((int)reader["EmployeeId"], (string)reader["Name"]);
                        }
                    }
                }
            }
            return names;
        }

    }
}
