CREATE TABLE AttendanceRecord (
    RecordId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    Date DATE NOT NULL,
    CheckIn DATETIME NULL,
    CheckOut DATETIME NULL,
    WorkHours FLOAT NULL,
    IsLate BIT DEFAULT 0,
    IsEarlyLeave BIT DEFAULT 0,
    FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId) ON DELETE CASCADE
);
GO