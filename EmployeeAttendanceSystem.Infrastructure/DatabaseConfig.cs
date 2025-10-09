using System;
using System.Data.SqlClient;
using System.Configuration;

namespace EmployeeAttendanceSystem.Infrastructure
{
    public static class DatabaseConfig
    {
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["EmployeeAttendanceDB"]?.ConnectionString
            ?? throw new InvalidOperationException("找不到名為 'EmployeeAttendanceDB' 的連線字串設定");
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
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException($"設定檔錯誤: {ex.Message}", ex);
            }
        }

        public static string GetDatabaseName()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            return builder.InitialCatalog;
        }
    }
}
