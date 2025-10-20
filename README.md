# SnippetNet

SnippetNet is a layered ASP.NET Core Razor Pages application for capturing, organising, and sharing reusable code snippets. It combines a clean CQRS-style backend with a polished, search-friendly UI so teams can keep their favourite snippets close at hand.

## Features

- **Snippet library management** – Create, edit, view, and delete snippets with metadata for title, language, tag, and description, backed by MediatR commands/queries and Razor Pages forms.
- **Instant search and empty-state guidance** – Filter the snippet grid client-side as you type and display contextual guidance when nothing matches.
- **Copy-ready detail views** – Present beautifully formatted code blocks with a clipboard shortcut and toast feedback for quick sharing.
- **Robust validation and error handling** – FluentValidation rules guard command inputs and MediatR pipeline behaviors provide centralised validation and logging for unhandled exceptions.
- **SQL Server persistence with migrations** – Entity Framework Core models, configurations, and migrations manage the Snippets table and audit columns.
- **Polished developer experience** – Modern navigation, hero section, and themed components styled with Bootstrap 5, Bootstrap Icons, and custom glassmorphism-inspired CSS.

## Architecture

SnippetNet follows a clean architecture-inspired layout with strict boundaries between layers:

```
src/
├─ SnippetNet.Domain          // Entity definitions and shared abstractions
├─ SnippetNet.Application     // CQRS handlers, DTOs, validators, pipeline behaviours
├─ SnippetNet.Infrastructure  // EF Core persistence, repositories, migrations
└─ SnippetNet.WebApp          // Razor Pages UI, dependency injection, static assets
```

- **Domain** – Contains the `Snippet` aggregate and base entity abstractions, keeping persistence concerns out of the core model.
- **Application** – Implements commands/queries with MediatR, DTO mapping, FluentValidation, and cross-cutting pipeline behaviours.
- **Infrastructure** – Provides the EF Core `ApplicationDbContext`, repositories, and unit-of-work implementations registered through DI.
- **WebApp** – Hosts the Razor Pages front-end, registers the application/infrastructure services, and defines the interactive UI flows.

## Prerequisites

- [.NET SDK 9.0](https://dotnet.microsoft.com/download) (builds all projects targeting `net9.0`).
- SQL Server instance reachable from your development machine (Express, LocalDB, or container).
- Optional: [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) tool for applying migrations from the command line.

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-org/SnippetNet.git
   cd SnippetNet
   ```

2. **Configure the database connection**
   - Update `ConnectionStrings:Default` in `src/SnippetNet.WebApp/appsettings.json`, or
   - Use [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) during development:
     ```bash
     dotnet user-secrets init --project src/SnippetNet.WebApp
     dotnet user-secrets set "ConnectionStrings:Default" "Server=(localdb)\\MSSQLLocalDB;Database=SnippetNet;Trusted_Connection=True;TrustServerCertificate=True"
     ```

3. **Apply the database migrations**
   ```bash
   dotnet ef database update --project src/SnippetNet.Infrastructure --startup-project src/SnippetNet.WebApp
   ```

4. **Run the web application**
   ```bash
   dotnet run --project src/SnippetNet.WebApp
   ```
   Visit `https://localhost:5001` (or the URL printed in the console) to access the app.

## Common Tasks

- **Add a migration**
  ```bash
  dotnet ef migrations add <MigrationName> --project src/SnippetNet.Infrastructure --startup-project src/SnippetNet.WebApp
  ```

- **Update the database schema**
  ```bash
  dotnet ef database update --project src/SnippetNet.Infrastructure --startup-project src/SnippetNet.WebApp
  ```

- **Run in Docker (optional)** – A Dockerfile is provided for containerised builds using the .NET 9 runtime on Windows Nano Server.

## Coding Standards & Tooling Highlights

- CQRS with MediatR keeps queries and commands focused and testable.
- FluentValidation ensures consistent, declarative validation rules across forms.
- Pipeline behaviors centralise validation and exception logging concerns.
- Repository + Unit of Work abstractions encapsulate EF Core specifics, simplifying handler logic.
- Rich UI built with Bootstrap 5, Bootstrap Icons, and custom CSS for a cohesive developer-facing experience.

## Project Status

This repository includes end-to-end functionality for managing snippets but does not yet ship with automated tests. Contributions that add test coverage, extend snippet metadata, or introduce advanced search/filtering are welcome.
