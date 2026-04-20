# PantawidPasada

A Windows Forms desktop application built with **.NET 10** and **C#** that manages subsidy applications, fare matrices, fuel prices, and account management for jeepney drivers, administrators, and government officials.

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Clone the Repository](#clone-the-repository)
3. [Database Setup (MySQL — no containerization)](#database-setup-mysql--no-containerization)
4. [Update Connection Strings](#update-connection-strings)
5. [Restore NuGet Packages & Build](#restore-nuget-packages--build)
6. [Running the App](#running-the-app)
7. [Project Structure](#project-structure)
8. [Roles & Default Accounts](#roles--default-accounts)

---

## Prerequisites

| Tool | Version | Download |
|------|---------|----------|
| Visual Studio 2022 (or later) | 17.x+ | https://visualstudio.microsoft.com/ |
| .NET SDK | 10.0 | https://dotnet.microsoft.com/download |
| MySQL Community Server | 8.x | https://dev.mysql.com/downloads/mysql/ |
| MySQL Workbench *(optional but recommended)* | 8.x | https://dev.mysql.com/downloads/workbench/ |

> **Visual Studio Workloads required:**
> - `.NET desktop development`

---

## Clone the Repository

```bash
git clone https://github.com/En1gM-a/PantawidPasada2.git
cd PantawidPasada2
```

---

## Database Setup (MySQL — no containerization)

### 1. Install & start MySQL

During MySQL installation, set a root password you will remember.
Make sure the MySQL service is running:

- **Windows Services** → find `MySQL80` → set to *Automatic* and *Start*
- Or via command prompt (run as Administrator):

```cmd
net start MySQL80
```

### 2. Create the database

Open **MySQL Workbench** (or the MySQL command-line client) and run:

```sql
CREATE DATABASE IF NOT EXISTS pantawid_pasada
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE pantawid_pasada;
```

---

### 3. Create the tables

Copy and run the following SQL to create all required tables:

```sql
-- --------------------------------------------------------
-- Driver accounts
-- --------------------------------------------------------
CREATE TABLE IF NOT EXISTS driverAccs (
    driver_id        INT            NOT NULL AUTO_INCREMENT,
    first_name       VARCHAR(100)   NOT NULL,
    last_name        VARCHAR(100)   NOT NULL,
    middle_name      VARCHAR(100)   DEFAULT NULL,
    address          VARCHAR(255)   NOT NULL,
    province         VARCHAR(100)   NOT NULL,
    phone_num        VARCHAR(20)    NOT NULL,
    email            VARCHAR(150)   NOT NULL,
    usernameUser     VARCHAR(100)   NOT NULL UNIQUE,
    passwordUser     VARCHAR(255)   NOT NULL,
    income           VARCHAR(100)   DEFAULT NULL,
    employment_type  VARCHAR(100)   DEFAULT NULL,
    source_of_income VARCHAR(150)   DEFAULT NULL,
    finan_ob         VARCHAR(255)   DEFAULT NULL,
    plate_number     VARCHAR(50)    DEFAULT NULL,
    lic_num          VARCHAR(50)    DEFAULT NULL,
    vehicle_type     VARCHAR(100)   DEFAULT NULL,
    subsidy_stats    VARCHAR(50)    DEFAULT 'Pending',
    created_at       DATETIME       DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (driver_id)
) ENGINE=InnoDB;
```

```sql
-- --------------------------------------------------------
-- Admin accounts
-- --------------------------------------------------------
CREATE TABLE IF NOT EXISTS admins (
    AdminID        INT           NOT NULL AUTO_INCREMENT,
    FirstName      VARCHAR(100)  NOT NULL,
    LastName       VARCHAR(100)  NOT NULL,
    MiddleInitial  VARCHAR(10)   DEFAULT NULL,
    RoleAdmin      VARCHAR(50)   DEFAULT 'Admin',
    UsernameAdmin  VARCHAR(100)  NOT NULL UNIQUE,
    PasswordAdmin  VARCHAR(255)  NOT NULL,
    adminStatus    VARCHAR(20)   DEFAULT 'Active',
    contactNum     VARCHAR(20)   DEFAULT NULL,
    email          VARCHAR(150)  DEFAULT NULL,
    PRIMARY KEY (AdminID)
) ENGINE=InnoDB;

-- --------------------------------------------------------
-- Government official accounts
-- --------------------------------------------------------
CREATE TABLE IF NOT EXISTS govAccs (
    GovID          INT           NOT NULL AUTO_INCREMENT,
    FirstName      VARCHAR(100)  NOT NULL,
    LastName       VARCHAR(100)  NOT NULL,
    MiddleInitial  VARCHAR(10)   DEFAULT NULL,
    Agency         VARCHAR(100)  NOT NULL,   -- LTFRB | LTO | DOTr | DSWD
    Username       VARCHAR(100)  NOT NULL UNIQUE,
    Password       VARCHAR(255)  NOT NULL,
    govStatus      VARCHAR(20)   DEFAULT 'Active',
    contactNum     VARCHAR(20)   DEFAULT NULL,
    email          VARCHAR(150)  DEFAULT NULL,
    PRIMARY KEY (GovID)
) ENGINE=InnoDB;
```

> **Note:** If your project uses additional tables (e.g. `fareMatrix`, `fuelPrices`), add their `CREATE TABLE` statements here.  
> It is recommended to export your current schema from MySQL Workbench via  
> **Server → Data Export → Export to Self-Contained File** and commit that `.sql` file to the repo.

---

### 4. Insert a default admin account

```sql
USE pantawid_pasada;

INSERT INTO admins
  (FirstName, LastName, MiddleInitial, RoleAdmin, UsernameAdmin, PasswordAdmin, adminStatus, contactNum, email)
VALUES
  ('Super', 'Admin', '', 'Admin', 'admin@super.admin', 'admin1234', 'Active', '', '');
```

> Change the username and password immediately after first login.

---

## Update Connection Strings

The database connection string is currently hardcoded in several `.cs` files.  
On a new machine, open each file below and replace the credentials with your own MySQL root password:

| File | Location of `connStr` |
|------|-----------------------|
| `SaveDataBase.cs` | Line ~8 |
| `loginCheck.cs` | Line ~8 |
| `manageAccAdmin.cs` | Multiple methods |
| `accessAdminGovAccs.cs` | Check for `connStr` |
| `accessDriverInfo.cs` | Check for `connStr` |
| `subsidyApp.cs` | `LoadDriverDetails()` |

Change every occurrence of:

```csharp
string connStr = "server=localhost;user id=root;password=YOUR_OLD_PASSWORD;database=pantawid_pasada;";
```

to:

```csharp
string connStr = "server=localhost;user id=root;password=YOUR_NEW_PASSWORD;database=pantawid_pasada;";
```

> ⚠️ **Security reminder:** Hardcoding credentials in source files is a security risk.  
> Consider moving the connection string to a local `appsettings.local.json` file (already in `.gitignore`)  
> and reading it via `System.IO.File.ReadAllText()` or `Microsoft.Extensions.Configuration`.

---

## Restore NuGet Packages & Build

### Using Visual Studio

1. Open `PantawidPasada.slnx` in Visual Studio 2022.
2. Right-click the solution in **Solution Explorer** → **Restore NuGet Packages**.
3. Press **Ctrl+Shift+B** to build.

### Using the command line

```bash
dotnet restore
dotnet build
```

NuGet packages used by this project:

| Package | Purpose |
|---------|---------|
| `MySql.Data` | MySQL database connector |
| `LiveChartsCore` + `LiveChartsCore.SkiaSharpView.WinForms` | Charts and graphs |
| `Newtonsoft.Json` | JSON serialization |
| `Parquet.Net` | Parquet file support |
| `SkiaSharp` | 2D graphics (required by LiveCharts) |
| `System.Net.Http` | HTTP requests (fuel price fetching) |

---

## Running the App

Press **F5** in Visual Studio, or:

```bash
dotnet run
```

The login screen will appear. Use the admin credentials you inserted in the database setup step.

---

## Project Structure

```
PantawidPasada/
├── Program.cs                  # Entry point
├── PantawidPasada.csproj       # Project file (.NET 10, WinForms)
├── PantawidPasada.slnx         # Solution file
│
├──── Forms (UI) ──────────────────────────────────────────────
├── Form1.cs / Form2.cs / Form3.cs   # Splash / Login screens
├── adminPanel.cs               # Main shell for Admin role
├── governmentPanel.cs          # Main shell for Government role
├── homeAdmin.cs                # Admin home dashboard
├── homeDriver.cs               # Driver home dashboard
├── homeGovernment.cs           # Government home dashboard
│
├── ── Feature Panels ─────────────────────────────────────────
├── subsidyApp.cs               # Subsidy application management
├── fareMatrix.cs               # Jeepney fare matrix
├── fuelPrice.cs                # Fuel price display
├── fuelPriceONLINE.cs          # Live fuel price fetcher
├── fuelPricewithStation.cs     # Fuel price per station
├── manageFuel.cs               # Fuel price CRUD
├── manageAccAdmin.cs           # Admin/Gov account management
├── personalInfo.cs             # Driver personal info
├── vehicleInfo.cs              # Driver vehicle info
├── financialInfo.cs            # Driver financial info
├── contact.cs                  # Contact / support page
├── summary.cs                  # Summary / reports
├── jeepFare1.cs                # Jeepney fare calculator
├── forGraph.cs                 # Graph/chart helper
│
├── ── Data / Logic ────────────────────────────────────────────
├── SaveDataBase.cs             # DB write helper (driver registration)
├── loginCheck.cs               # Login authentication (all 3 roles)
├── accessAdminGovAccs.cs       # DataGrid loader for admins & gov
├── accessDriverInfo.cs         # DataGrid loader for drivers
├── UserData.cs                 # Driver data model
├── adminAcc.cs                 # Admin data model
├── govData.cs                  # Government official data model
├── fuelPriceData.cs            # Fuel price model + fare calc
├── SlantedButton.cs            # Custom UI control
│
├── ── Designer / Resource files ───────────────────────────────
├── *.Designer.cs               # Auto-generated WinForms layout
├── *.resx                      # Embedded resources per form
├── Properties/                 # App-level resources & settings
└── Resources/                  # Images and assets
```

---

## Roles & Default Accounts

The application has **three login roles**:

| Role | Table | Notes |
|------|-------|-------|
| **Admin** | `admins` | Full access: manage accounts, subsidies, fuel prices |
| **Government Official** | `govAccs` | Agency-level access (LTFRB, LTO, DOTr, DSWD) |
| **Driver** | `driverAccs` | Self-service: view subsidy status, fare info, fuel prices |

Driver accounts are created through the app's registration flow.  
Admin and Government accounts are created by an existing Admin from the **Manage Accounts** panel.

---

## Common Issues

| Problem | Fix |
|---------|-----|
| `Unable to connect to MySQL` | Make sure `MySQL80` service is running and credentials in `connStr` match your local setup |
| `Unknown database 'pantawid_pasada'` | Run the `CREATE DATABASE` statement from Step 2 above |
| `Table doesn't exist` | Run all `CREATE TABLE` statements from Step 3 |
| Build error: missing NuGet packages | Run **Restore NuGet Packages** in Visual Studio or `dotnet restore` |
| App targets wrong .NET version | Install .NET 10 SDK from https://dotnet.microsoft.com/download |
