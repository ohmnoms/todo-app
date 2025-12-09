# todo-app
Small TODO app showcase using Vue and .NET Core

This app is built with a .NET 10 ASP.NET Core backend and Vue 3 frontend as a monorepo app.

## Referenced Documentation & Utilities

- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/overview?view=aspnetcore-10.0)
- [Vue.js](https://vuejs.org/guide/quick-start)
- [daisyUI](https://daisyui.com/docs)
- [tailwindcss](https://tailwindcss.com/docs/styling-with-utility-classes)
- [Google Material Icons](https://fonts.google.com/icons)
- [png2ico](https://www.png2ico.com/) to create favicon.ico

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

## Frontend
The frontend is a modern Vue 3 application built with TypeScript and styled using Tailwind CSS with daisyUI components. It provides an intuitive interface for managing todo items with real-time updates and responsive design.

### Architecture Overview

The application follows Vue 3 Composition API best practices:
- **Components** - Reusable UI components using `<script setup>` syntax
- **Services** - API communication layer
- **Composables** - Shared reactive logic and state management
- **Types** - TypeScript definitions for type safety

### Key Components

#### Main Application
- **[App.vue](frontend/todo-app/src/App.vue)**
  - Root application component
  - Provides global layout structure
  - Manages application-wide state and theme

#### Core Components
- **[TodoList.vue](frontend/todo-app/src/components/TodoList.vue)**
  - Main component for displaying the list of todo items
  - Implements infinite scrolling or pagination
  - Uses daisyUI `list` component for clean presentation
  - Handles loading states with `skeleton` components

- **[TodoItem.vue](frontend/todo-app/src/components/TodoItem.vue)**
  - Individual todo item component with inline editing
  - Toggle completion status with daisyUI `checkbox`
  - Delete functionality with confirmation `modal`
  - Responsive design with `card` component

- **[AddTodo.vue](frontend/todo-app/src/components/AddTodo.vue)**
  - Form component for creating new todo items
  - Uses daisyUI `input` and `btn` components
  - Client-side validation with error states
  - Keyboard shortcuts (Enter to submit)

#### UI Components
- **[LoadingSpinner.vue](frontend/todo-app/src/components/ui/LoadingSpinner.vue)**
  - Reusable loading component using daisyUI `loading` styles
  - Multiple animation variants (spinner, dots, bars)

- **[ErrorAlert.vue](frontend/todo-app/src/components/ui/ErrorAlert.vue)**
  - Error display component using daisyUI `alert` with error styling
  - Dismissible with fade animations

#### Services
- **[todoService.ts](frontend/todo-app/src/services/todoService.ts)**
  - API communication layer using native `fetch`
  - Handles all CRUD operations for todo items
  - Error handling and response transformation
  - TypeScript interfaces for API responses

#### Composables
- **[useTodos.ts](frontend/todo-app/src/composables/useTodos.ts)**
  - Main state management composable
  - Reactive todo list with computed properties
  - CRUD operations with optimistic updates
  - Loading and error state management

- **[useApi.ts](frontend/todo-app/src/composables/useApi.ts)**
  - Generic API composable for HTTP operations
  - Request/response interceptors
  - Error handling and retry logic

#### Types
- **[todo.types.ts](frontend/todo-app/src/types/todo.types.ts)**
  - TypeScript interfaces matching backend contracts
  - `TodoItem`, `CreateTodoRequest`, `UpdateTodoRequest` types
  - API response and error types

### Styling and Theming

The application uses Tailwind CSS with daisyUI for consistent, accessible components:
- **Responsive Design** - Mobile-first approach with `sm:`, `md:`, `lg:` breakpoints
- **Theme Support** - Light/dark mode with `theme-controller` component
- **Color System** - daisyUI semantic colors (`primary`, `secondary`, `accent`, etc.)
- **Typography** - Consistent text sizing and spacing

### Key Features

#### User Interface
- Clean, modern design with daisyUI components
- Responsive layout that works on all screen sizes
- Accessibility-first approach with proper ARIA labels
- Smooth animations and transitions

#### Functionality
- Real-time todo management (create, read, update, delete)
- Mark todos as complete/incomplete
- Persistent state with backend synchronization
- Error handling with user-friendly messages
- Loading states during API operations

#### Developer Experience
- TypeScript for type safety and better IDE support
- Vue 3 Composition API with `<script setup>`
- Hot module replacement for fast development
- ESLint and Prettier for code consistency

### Technology Stack
- Vue 3 with Composition API
- TypeScript for type safety
- Vite for build tooling and development server
- Tailwind CSS for utility-first styling
- daisyUI for pre-built accessible components
- ESLint and Prettier for code quality

### Development Setup
The frontend development server integrates seamlessly with the backend API through Vite's proxy configuration, enabling full-stack development with hot reload capabilities.