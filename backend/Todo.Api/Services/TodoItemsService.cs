using System.Security.Authentication;
using Todo.Api.Models.Contracts;
using Todo.Api.Models.Domain;
using Todo.Api.Persistence;
using Todo.Api.Validation;

namespace Todo.Api.Services;

public interface ITodoItemsService
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(GetTodoRequest? request = null, CancellationToken ct = default);
    Task<TodoItem> GetByIdAsync(Guid id, GetTodoRequest? request = null, CancellationToken ct = default);
    Task<TodoItem> CreateAsync(CreateTodoRequest request, CancellationToken ct = default);
    Task<TodoItem> UpdateAsync(Guid id, UpdateTodoRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}

public class TodoItemsService(ITodoItemsRepository repository) : ITodoItemsService
{
    private readonly ITodoItemsRepository _repository = repository;

    /// <summary>
    /// Gets all Todo items. This is currently filtered by CreatedBy to only get client user's todos.
    /// You cannot call this method to get another user's Todo items.
    /// </summary>
    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(GetTodoRequest? request, CancellationToken ct = default)
    {
        if (request is null || request.CreatedBy is null)
        {
            throw new AuthenticationException("CreatedBy is required to get Todo items.");
        }
        return await _repository.GetAllAsync(request, ct);
    }

    /// <summary>
    /// Gets Todo item by ID. 
    /// This is currently filtered by CreatedBy to only get client user's todos.
    /// You cannot call this method to get another user's Todo item.
    /// </summary>
    public async Task<TodoItem> GetByIdAsync(Guid id, GetTodoRequest? request, CancellationToken ct = default)
    {
        if (request is null || request.CreatedBy is null)
        {
            throw new AuthenticationException("CreatedBy is required to get Todo items.");
        }
        var todo = await _repository.GetByIdAsync(id, request, ct) ??
            throw new KeyNotFoundException($"Todo item with id {id} not found.");
        return todo;
    }

    public async Task<TodoItem> CreateAsync(CreateTodoRequest request, CancellationToken ct = default)
    {
        await TodoItemValidator.ValidateTitleAsync(request.Title, ct);
        await TodoItemValidator.ValidateDueDateAsync(request.DueDate, ct);
        await TodoItemValidator.ValidateDueTimeAsync(request.DueTime, ct);

        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            IsCompleted = false,
            CreatedDate = DateTimeOffset.Now,
            DueDate = request.DueDate,
            DueTime = request.DueTime,
            CreatedBy = request.CreatedBy
        };

        await _repository.AddAsync(todo, ct);
        return todo;
    }

    public async Task<TodoItem> UpdateAsync(Guid id, UpdateTodoRequest request, CancellationToken ct = default)
    {
        var todo = await _repository.GetByIdAsync(id, ct: ct) 
            ?? throw new KeyNotFoundException($"Todo item with id {id} not found.");

        await TodoItemValidator.ValidateTitleAsync(request.Title, ct);
        await TodoItemValidator.ValidateDueDateAsync(request.DueDate, ct);
        await TodoItemValidator.ValidateDueTimeAsync(request.DueTime, ct);
        
        todo.Title = request.Title;
        todo.IsCompleted = request.IsCompleted;
        todo.CompletedDate = request.CompletedDate;
        todo.DueDate = request.DueDate;
        todo.DueTime = request.DueTime;

        await _repository.UpdateAsync(todo, ct);
        return todo;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var todo = await _repository.GetByIdAsync(id, ct: ct) 
            ?? throw new KeyNotFoundException($"Todo item with id {id} not found.");

        await _repository.DeleteAsync(todo, ct);
        return true;
    }
}