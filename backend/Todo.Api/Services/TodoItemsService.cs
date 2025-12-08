using Todo.Api.Models.Contracts;
using Todo.Api.Models.Domain;
using Todo.Api.Persistence;
using Todo.Api.Validation;

namespace Todo.Api.Services;

public interface ITodoItemsService
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default);
    Task<TodoItem> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TodoItem> CreateAsync(CreateTodoRequest request, CancellationToken ct = default);
    Task<TodoItem> UpdateAsync(Guid id, UpdateTodoRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}

public class TodoItemsService(ITodoItemsRepository repository) : ITodoItemsService
{
    private readonly ITodoItemsRepository _repository = repository;

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(ct);
    }

    public async Task<TodoItem> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var todo = await _repository.GetByIdAsync(id, ct) ??
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
            CreatedDate = DateTimeOffset.Now
        };

        await _repository.AddAsync(todo, ct);
        return todo;
    }

    public async Task<TodoItem> UpdateAsync(Guid id, UpdateTodoRequest request, CancellationToken ct = default)
    {
        var todo = await _repository.GetByIdAsync(id, ct) 
            ?? throw new KeyNotFoundException($"Todo item with id {id} not found.");

        await TodoItemValidator.ValidateTitleAsync(request.Title, ct);
        await TodoItemValidator.ValidateDueDateAsync(request.DueDate, ct);
        await TodoItemValidator.ValidateDueTimeAsync(request.DueTime, ct);
        
        todo.Title = request.Title;
        todo.IsCompleted = request.IsCompleted;

        await _repository.UpdateAsync(todo, ct);
        return todo;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var todo = await _repository.GetByIdAsync(id, ct) 
            ?? throw new KeyNotFoundException($"Todo item with id {id} not found.");

        await _repository.DeleteAsync(todo, ct);
        return true;
    }
}