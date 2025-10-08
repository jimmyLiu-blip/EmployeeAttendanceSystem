using EmployeeAttendanceSystem.Domain;
using EmployeeAttendanceSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace EmployeeAttendanceSystem.Repository
{
    public class SqlAttendanceRepository:IAttendanceRepository
    {
        private AttendanceRecord MapToAttendance(SqlDataReader reader)
        {
            return new AttendanceRecord
            {
                RecordId = (int)reader["RecordId"],
                EmployeeId = (int)reader["EmployeeId"],
                Date = (DateTime)reader["Date"],
                CheckIn = reader["CheckIn"] == DBNull.Value ? null:(DateTime?)reader["CheckIn"],
                CheckOut = reader["CheckOut"] == DBNull.Value ? null : (DateTime?)reader["CheckOut"],
                WorkHours = reader["WorkHours"] == DBNull.Value ? null : (decimal?)reader["WorkHours"],
                IsLate = (bool)reader["IsLate"],
                IsEarlyLeave = (bool)reader["IsEarlyLeave"]
            };
        }
        public SqlAttendanceRepository() { }
        public void AddAttendanceRecord(AttendanceRecord attendancerecord)
        {
            using (var cn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"INSERT INTO AttendanceRecord (EmployeeId, Date, CheckIn, CheckOut, WorkHours, IsLate, IsEarlyLeave)
                            OUTPUT INSERTED.RecordId
                            VALUES(@EmployeeId, @Date, @CheckIn, @CheckOut, @WorkHours, @IsLate, @IsEarlyLeave )";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@EmployeeId", System.Data.SqlDbType.Int).Value = attendancerecord.EmployeeId;
                    cmd.Parameters.Add("@Date", System.Data.SqlDbType.Date).Value = attendancerecord.Date;
                    cmd.Parameters.Add("@CheckIn", System.Data.SqlDbType.DateTime).Value = (object) attendancerecord.CheckIn ?? DBNull.Value;
                    cmd.Parameters.Add("@CheckOut", System.Data.SqlDbType.DateTime).Value = (object) attendancerecord.CheckOut ?? DBNull.Value;
                    cmd.Parameters.Add("@WorkHours", System.Data.SqlDbType.Decimal).Value = (object) attendancerecord.WorkHours ?? DBNull.Value;
                    cmd.Parameters.Add("@IsLate", System.Data.SqlDbType.Bit).Value = attendancerecord.IsLate;
                    cmd.Parameters.Add("@IsEarlyLeave", System.Data.SqlDbType.Bit).Value = attendancerecord.IsEarlyLeave;
                    cn.Open();

                    var newId = cmd.ExecuteScalar();
                    if (newId != null && newId != DBNull.Value)
                    {
                        attendancerecord.RecordId = (int)newId;
                    }
                }
            }
        }
        public AttendanceRecord GetRecordByEmployeeIdAndDate(int employeeId, DateTime date)
        {
            using (var cn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"SELECT RecordId, EmployeeId, Date, CheckIn, CheckOut, WorkHours, IsLate, IsEarlyLeave 
                            FROM AttendanceRecord 
                            WHERE EmployeeId = @EmployeeId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@EmployeeId", System.Data.SqlDbType.Int).Value = employeeId;
                    cmd.Parameters.Add("@Date", System.Data.SqlDbType.Date).Value = date.Date;
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapToAttendance(reader);
                        }
                        return null;
                    }
                }
            }
        }
        public IEnumerable<AttendanceRecord> GetAllEmployeesTodayRecords()
        { 
            DateTime date = DateTime.Today.Date;

            var allEmployeesTodayRecords = new List<AttendanceRecord>();

            using (var cn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"SELECT RecordId, EmployeeId, Date, CheckIn, CheckOut, WorkHours, IsLate, IsEarlyLeave 
                            FROM AttendanceRecord
                            WHERE Date = @TodayDate";

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@TodayDate", System.Data.SqlDbType.Date).Value = date;
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allEmployeesTodayRecords.Add(MapToAttendance(reader));
                        }
                    }
                }
            }
            return allEmployeesTodayRecords;
        }
        public IEnumerable<AttendanceRecord> GetRecordsByEmployeeId(int employeeId)
        { 
            var recordByEmployeeId = new List<AttendanceRecord>();
            using (var cn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"SELECT RecordId, EmployeeId, Date, CheckIn, CheckOut, WorkHours, IsLate, IsEarlyLeave 
                            FROM AttendanceRecord
                            WHERE EmployeeId = @EmployeeId";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@EmployeeId", System.Data.SqlDbType.Int).Value = employeeId;
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        { 
                            recordByEmployeeId.Add(MapToAttendance(reader));
                        }
                    }
                }
            }
            return recordByEmployeeId;
        }
        public void UpdateAttendanceRecord(AttendanceRecord record)
        {
            using (var cn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"UPDATE AttendanceRecord SET 
                            EmployeeId = @EmployeeId,
                            Date = @Date,
                            CheckIn = @CheckIn,
                            CheckOut = @CheckOut,
                            WorkHours = @WorkHours,
                            IsLate = @IsLate,
                            IsEarlyLeave = @IsEarlyLeave
                            WHERE RecordId = @RecordId";
                using (var cmd = new SqlCommand(sql, cn))
                { 
                    cmd.Parameters.Add("@EmployeeId", System.Data.SqlDbType.Int).Value = record.EmployeeId;
                    cmd.Parameters.Add("@Date", System.Data.SqlDbType.Date).Value = record.Date;
                    cmd.Parameters.Add("@CheckIn", System.Data.SqlDbType.DateTime).Value = (object) record.CheckIn ?? DBNull.Value;
                    cmd.Parameters.Add("@CheckOut", System.Data.SqlDbType.DateTime).Value = (object)record.CheckOut ?? DBNull.Value;
                    cmd.Parameters.Add("@WorkHours", System.Data.SqlDbType.Decimal).Value = (object)record.WorkHours ?? DBNull.Value;
                    cmd.Parameters.Add("@IsLate", System.Data.SqlDbType.Bit).Value = record.IsLate;
                    cmd.Parameters.Add("@IsEarlyLeave", System.Data.SqlDbType.Bit).Value = record.IsEarlyLeave;
                    cmd.Parameters.Add("@RecordId", System.Data.SqlDbType.Int).Value = record.RecordId;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteAttendanceRecord(int recordId)
        {
            using (var cn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                var sql = @"DELETE AttendanceRecord
                            WHERE RecordId = @RecordId";
                using (var cmd = new SqlCommand(sql, cn))
                { 
                    cmd.Parameters.Add("@RecordId", System.Data.SqlDbType.Int).Value = recordId;
                    cn.Open() ;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
