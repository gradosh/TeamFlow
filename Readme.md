# TeamFlow

Backend API для управления проектами и задачами (Kanban-style).  
Реализован с использованием Clean Architecture и CQRS.

## Stack

- .NET 10 (ASP.NET Core Web API)
- Clean Architecture
- CQRS + MediatR
- FluentValidation (Pipeline)
- AutoMapper
- Entity Framework Core
- PostgreSQL
- Redis
- JWT authentication
- Role-based authorization
- Domain Events
- Docker / Docker Compose
- Testcontainers (integration tests)

## Features

- JWT authentication
- Project & Task management
- Kanban board (Todo / InProgress / Done)
- Owner-based authorization
- Redis caching (Cache-aside via MediatR Pipeline)
- Global Exception Middleware

## Architecture

Clean Architecture:

- **Domain** — entities & business logic  
- **Application** — CQRS, interfaces, behaviors  
- **Infrastructure** — EF Core, Redis, repositories  
- **API** — controllers, middleware, DI  

## Caching

- Redis
- User & Project scoped keys
- TTL expiration
- Automatic caching via MediatR pipeline
- Explicit cache invalidation in commands

## Implemented patterns:
- CQRS
- Pipeline behaviors
- Event-driven cache invalidation
- Role-based and resource-based authorization

## Run

```bash
docker-compose up -d
dotnet ef database update
dotnet run
```

---

MIT License
