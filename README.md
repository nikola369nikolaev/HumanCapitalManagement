# Human Capital Management App

A web application for managing human capital in an organization – includes user authentication, role-based access, employee and department management.

---

## Table of Contents

- [Features](#features)
- [User Roles](#user-roles)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Database & Migrations](#database--migrations)
- [Docker Support](#docker-support)
- [Testing](#testing)
- [Notes](#notes)

---

## Features

-  User registration and login (ASP.NET Identity)
-  Role-based authorization (Employee, Manager, HR Admin)
-  CRUD for Employees and Departments
-  Razor Pages layout with role-specific access
-  Secure Logout
-  Dockerized deployment
-  Unit Testing for Account actions

---

## User Roles

| Role       | Permissions                                                  |
|------------|--------------------------------------------------------------|
| **EMPLOYEE** | View their own profile only                                 |
| **MANAGER**  | View and edit all employees in their department             |
| **HR ADMIN** | Full access to all records (view, edit, delete, assign)     |

Roles are seeded at startup and assigned manually (or via admin logic).

---

## Tech Stack

- **Frontend:** Razor Pages, Bootstrap
- **Backend:** ASP.NET Core 8.0
- **Authentication:** ASP.NET Identity
- **Database:** SQL Server + EF Core (Code-First), SQLite in Unit Tests
- **Testing:** xUnit + Moq
- **Containerization:** Docker

---

## Project Structure

```
HumanCapitalManagement/
│
├── Controllers/ # Interfaces and domain logic
├── Data/ # DbContext, migrations
├── Models/ # ApplicationUser, Employee, Department
├── Views/ # Razor Pages (UI)
├── Services/ # Business logic services
├── Program.cs # Startup configuration
└── appsettings.json # Connection strings & Identity settings

HumanCapitalManagement.Tests/
├── # Unit tests
```

## Getting Started

```bash

# Clone the repository
git clone https://github.com/nikola369nikolaev/human-capital-management.git
cd human-capital-management

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the app
dotnet run

```

## Database & Migrations

The application uses **Entity Framework Core** with **SQL Server** for data persistence.

### Database Context  
The main DbContext is defined in:  
/HumanCapitalManagement/Data/ApplicationDbContext.cs  
```bash

It includes the following DbSets:
- `ApplicationUser` – extends `IdentityUser` for custom user data
- `Employee` – represents employee records
- `Department` – represents company departments

### Connection String
Configured in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HumanCapitalManagementDb;Trusted_Connection=True;"
}
```

## Docker Support

```bash

# Build the Docker image
docker build -t human-capital-management .

# Run the container
docker run -d -p 5140:80 human-capital-management

```

## Testing

Unit tests are located in the `HumanCapitalManagement.Tests/` project.

### Test Structure

HumanCapitalManagement.Tests/  
├── Accounts/  
│ └── AccountTests.cs               # Tests for user registration and login  
├── Services/  
│ └── DepartmentServiceTests.cs  
│ └── AccountServiceTests.cs        # Tests for service-layer logic  
│ └── EmployeeServiceTests.cs  
├── Validation/                     # Tests for model validation  
│ └── InputModelValidationTests.cs  

The tests are written using **xUnit** and **Moq**, and executed with an **in-memory SQLite database** to simulate real  
interactions without requiring a full database server.  

- Account registration & login  
- Input model validation logic  
- Service-level functionality  

## Notes

Login/Register UI is integrated into the **layout** (_LoginPartial.cshtml)  
Links to **/Employees** and **/Departments** are visible only to logged-in users  

```bash

HR Admin account login details:
email: hr.admin@example.com
pass: Hr.admin123!

Manager account login details:
email: manager@example.com
pass: Manager123!

Employee account login details:
This will be a valid employee login if the HR Admin created it with this email, and the password is the one shown.
email: georgi.hristov@gmail.com
pass: Employee123!

```
