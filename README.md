# todo-app
Small TODO app showcase using Vue and .NET Core

This app is built with a .NET 10 ASP.NET Core backend and Vue 3 frontend as a monorepo app.

## Referenced Documentation

- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/overview?view=aspnetcore-10.0)
- [Vue.js](https://vuejs.org/guide/quick-start)
- [daisyUI](https://daisyui.com/docs)
- [tailwindcss](https://tailwindcss.com/docs/styling-with-utility-classes)

## Backend

The backend is a .NET 10 ASP.NET Core Web API that provides RESTful endpoints for managing todo items. It follows a clean architecture pattern with clear separation of concerns.

### Architecture Overview

The application uses a layered architecture:
- **Controllers** - Handle HTTP requests and responses
- **Services** - Contain business logic
- **Persistence** - Data access layer with repository pattern
- **Models** - Domain entities and data transfer objects

### Key Components

#### Controllers
- **[TodoController](backend/src/Todo.Api/Controllers/TodoController.cs)**
    - RESTful API controller exposing CRUD operations for todo items
    - Endpoints: GET all todos, GET by id, POST create, PUT update, DELETE
    - Uses dependency injection to access `ITodoService`
    - Includes comprehensive ProducesResponseType attributes for OpenAPI documentation
  - RESTful API controller exposing CRUD operations for todo items
  - Endpoints: GET all todos, GET by id, POST create, PUT update, DELETE
  - Uses dependency injection to access `ITodoService`
  - Includes comprehensive ProducesResponseType attributes for OpenAPI documentation

#### Services
- **[TodoService](backend/src/Todo.Api/Services/TodoService.cs)** 
  - Implements business logic for todo operations
  - Handles validation and error handling (throws `KeyNotFoundException` for missing items)
  - Maps between domain models and request/response contracts
  - Uses `ITodoRepository` for data access

#### Persistence Layer
- **[TodoDbContext](backend/src/Todo.Api/Persistence/TodoDbContext.cs)**
  - Entity Framework Core DbContext managing the `TodoItems` DbSet
  - Configured to use in-memory database for development
  - Includes SQLite configuration option (commented out)
  - Applies entity configurations via fluent API

- **[TodoRepository](backend/src/Todo.Api/Persistence/TodoRepository.cs)**
  - Implements repository pattern via `ITodoRepository` interface
  - Provides data access methods: GetAll, GetById, Add, Update, Delete
  - Returns todos ordered by creation date
  - Handles all database operations asynchronously

- **[TodoItemConfiguration](backend/src/Todo.Api/Persistence/TodoConfiguration.cs)**
  - Entity Framework configuration for `TodoItem` entity
  - Defines table name, primary key, and property constraints
  - Sets `Title` as required with 240 character max length

#### Models
- **[Domain Models](backend/src/Todo.Api/Models/Domain/)**
  - **[TodoItem](backend/src/Todo.Api/Models/Domain/TodoItem.cs)** - Core domain entity with Id, Title, IsCompleted, CreatedDate, and CompletedDate properties
  - Implemented as a record type for immutability

- **[Contract Models](backend/src/Todo.Api/Models/Contracts/)**
  - **[CreateTodoRequest](backend/src/Todo.Api/Models/Contracts/CreateTodoRequest.cs)** - DTO for creating new todos
  - **[UpdateTodoRequest](backend/src/Todo.Api/Models/Contracts/UpdateTodoRequest.cs)** - DTO for updating existing todos
  - Implemented as record types for data transfer
#### Middleware
- **[ExceptionHandler](backend/src/Todo.Api/Common/ExceptionHandler.cs)**
    - Global exception handling middleware
    - Maps exceptions to appropriate HTTP status codes
    - Returns RFC 7807 Problem Details format for errors
    - Handles `KeyNotFoundException` (404), `ArgumentException` (400), and general exceptions (500)

### Configuration
- **[Program.cs](backend/src/Todo.Api/Program.cs)** - Application startup and configuration
  - Configures dependency injection (scoped services for repository and service layers)
  - Registers Entity Framework with in-memory database
  - Enables OpenAPI/Swagger for API documentation in development
  - Adds health check endpoint at `/health`
  - Configures global exception handling middleware

### Technology Stack
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10.0.0 with In-Memory database
- OpenAPI/Swagger for API documentation

