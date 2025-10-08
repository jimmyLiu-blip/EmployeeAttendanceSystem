using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceSystem.Infrastructure
{
    public static class DatabaseConfig
    {
        private const string DbName = "EmployeeAttendanceDB";

        public static readonly string ConnectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={DbName};Integrated Security=True;Timeout=30;";
        public static void TestConnection()
        {
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cn.Open();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"資料庫連線失敗，請檢查 LocalDB 實例和資料庫名稱。錯誤訊息: {ex.Message}", ex);
            }
        }
    }
}
