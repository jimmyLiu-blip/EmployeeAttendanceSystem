# 員工出勤系統 (Employee Attendance System) 專案計畫

---

## 1. 專案名稱

**「員工出勤系統」**

---

## 2. 專案目的

本專案旨在建立一個 **Console 應用程式**，模擬公司管理員工出勤的流程。

### 核心功能

* **員工管理：** 新增、查詢、修改、刪除員工資訊。
* **出勤記錄管理：** 新增打卡紀錄。

### 階段目標 (Minimum Viable Product, MVP)

#### **階段 1：基礎員工管理**

* **目的：** 建立員工資訊的 CRUD 功能 (新增、查詢、修改、刪除)。
* **技術重點：** 練習 **分層架構** 與 **資料存取分離** 的設計。

#### **階段 2：出勤功能強化**

* **目的：** 加入完整的出勤記錄管理。
* **功能：**
    * 新增打卡紀錄。
    * 自動判斷是否有 **遲到**、**早退**。
    * 計算 **上班總工時** 時數。

---

## 3. 流程分析 (Use Case / 流程細節)

### 主要流程

主選單 → 接收使用者指令 → 執行對應功能 → 顯示結果

### 主選單功能列表

| 選項 | 功能描述 |
| :--- | :--- |
| **1** | 新增員工資料 (Create) |
| **2** | 列出所有員工姓名 (Read) |
| **3** | 列出所有員工完整資料 (Read) |
| **4** | 透過員工編號取得員工資訊 (Read) |
| **5** | 更新員工資料 (Update) |
| **6** | 刪除員工資料 (Delete) |
| **7** | 新增打卡紀錄 (Attendance) |
| **8** | 離開系統 |

### 功能詳情 (使用案例分析)

#### 一、新增員工資料

1.  **使用者輸入：** 員工姓名 (**必填**)、部門 (選填)、職稱 (選填)。
2.  **系統處理：**
    * **Presentation Layer：** 檢查員工姓名輸入。
    * **Service Layer：** 檢查姓名是否為空（若為空則拋出例外）。
    * **Repository Layer：** 將資料寫入 SQL Server 的 `Employee` 資料表。
3.  **系統輸出：** 成功則顯示「員工資料新增成功」；失敗則顯示「員工姓名為必填，請重新輸入。」

#### 二、列出所有員工姓名

1.  **系統處理：**
    * **Service Layer：** 呼叫方法。
    * **Repository Layer：** 查詢員工編號和員工姓名的清單。
    * **Service Layer：** 檢查清單是否為空（若為空則拋出例外）。
2.  **系統輸出：** 成功則顯示「目前所有員工編號，員工姓名」；失敗則顯示「目前沒有任何員工資料」。

*（其他 CRUD 功能流程省略，設計邏輯與上述類似）*

#### 七、新增打卡紀錄

1.  **使用者輸入：** 員工編號。
2.  **系統處理：**
    * **Presentation Layer：** 嘗試將輸入轉換為 `int`。
    * **Service Layer：** 檢查員工編號是否有效。
    * **AttendanceService：**
        * 查詢當天是否有紀錄。
        * 若**無紀錄**，則計算**遲到**情況，存入新的上班紀錄。
        * 若**有紀錄**，則計算**上班工時**和是否**早退**，並更新下班時間。
3.  **系統輸出：** 成功則顯示「員工編號打卡成功，時間為：」；失敗則顯示「操作失敗，{ex.Message}。」

---

## 4. 程式架構設計 (分層式架構)

本專案採用經典的**三層式架構**：

| 層次 (Layer) | 職責 (Responsibility) | 相關類別 |
| :--- | :--- | :--- |
| **Presentation Layer (UI)** | 負責使用者輸入、輸出、顯示介面 (Console 應用程式)。 | `Program.cs` |
| **Business Logic Layer (Service)** | 核心商業邏輯、輸入檢查、流程控制。 | `EmployeeService`, `AttendanceService` |
| **Data Access Layer (Repository)** | 封裝資料存取邏輯、資料庫連線、SQL 指令操作。 | `SqlEmployeeRepository`, `SqlAttendanceRepository` |
| **Infrastructure Layer** | 資料庫連線設定等基礎設施配置。 | `DatabaseConfig` |
| **Domain Models** | 資料結構定義（物件）。 | `Employee`, `AttendanceRecord` |

```mermaid
graph TD
    A[Console (Presentation Layer)] -->|使用者輸入指令| B{Service Layer};
    B -->|Service 呼叫存取資料| C{Repository Layer};
    C -->|執行 SQL 指令 (透過 DatabaseConfig 連線)| D(SQL Server);
    D -->|回傳資料| C;
    C -->|回傳結果| B;
    B -->|最終輸出| A;

/EmployeeAttendanceSystem
├── Program.cs                      # 程式進入點 (Console)
│
├── Domoins/                        # 領域模型 (Domain Models / Entity)
│   └── Employee.cs                 # 員工資料模型
│   └── AttendanceRecord.cs         # 出勤紀錄資料模型
│
├── Infrastructure/                 # 基礎設施 (Infrastructure)
│   └── DatabaseConfig.cs           # 資料庫連線設定
│
├── Repositories/                   # 資料存取層 (Data Access Layer)
│   └── IEmployeeRepository.cs      # 員工資料存取介面
│   └── SqlEmployeeRepository.cs    # SQL Server 存取實作
│   └── IAttendanceRepository.cs    # 出勤紀錄資料存取介面
│   └── SqlAttendanceRepository.cs  # SQL Server 存取實作
│
└── Services/                       # 商業邏輯層 (Business Logic Layer)
    └── EmployeeService.cs          # 商業邏輯實作
    └── AttendanceService.cs        # 商業邏輯實作