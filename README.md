# UserManagement

A sample **ASP.NET Core Web API** for managing users (CRUD) built with **Clean Architecture**, **Entity Framework Core**, and **SQL Server**. This project is designed as a portfolio-ready example with clear separation of concerns, DI, and API documentation via Swagger.

## Features
- CRUD endpoints for Users
- Clean Architecture layering (API / Application / Domain / Infrastructure)
- EF Core + SQL Server integration
- Swagger/OpenAPI documentation
- CORS-ready for frontend apps (Angular/React)
- Health endpoint support (optional/if enabled)

## Tech Stack
- .NET 7 (ASP.NET Core Web API)
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI

## Project Structure (Clean Architecture)
Typical layout:
- **UserManagement.API** – Controllers, middleware, API configuration
- **UserManagement.Application** – DTOs, interfaces, business/services
- **UserManagement.Domain** – Entities and domain interfaces
- **UserManagement.Infrastructure** – EF Core DbContext, repositories, data access

## Getting Started

### Prerequisites
- .NET SDK 7+
- SQL Server (local or cloud)
- (Optional) EF Core tools:
  ```bash
  dotnet tool install --global dotnet-ef
