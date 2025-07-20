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

- ✅ User registration and login (ASP.NET Identity)
- ✅ Role-based authorization (Employee, Manager, HR Admin)
- ✅ CRUD for Employees and Departments
- ✅ Razor Pages layout with role-specific access
- ✅ Secure Logout
- ✅ Dockerized deployment
- ✅ Unit Testing for Account actions

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

## 📁 Project Structure
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

## 📁 Getting started
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
