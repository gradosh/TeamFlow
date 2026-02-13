# TeamFlow

Backend API для управления проектами и задачами (Kanban-style).  
Реализован с использованием Clean Architecture и CQRS.

## Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Redis
- MediatR (CQRS + Pipeline Behaviors)
- JWT Authentication
- Docker / Docker Compose

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

## Run

```bash
docker-compose up -d
dotnet ef database update
dotnet run
```

---

MIT License
