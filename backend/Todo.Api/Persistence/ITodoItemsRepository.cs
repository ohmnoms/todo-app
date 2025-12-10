using Todo.Api.Models.Contracts;
using Todo.Api.Models.Domain;

namespace Todo.Api.Persistence;

public interface ITodoItemsRepository
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(GetTodoRequest? request = null, CancellationToken ct = default);
    Task<TodoItem?> GetByIdAsync(Guid id, GetTodoRequest? request = null, CancellationToken ct = default);
    Task<TodoItem> AddAsync(TodoItem todo, CancellationToken ct = default);
    Task UpdateAsync(TodoItem todo, CancellationToken ct = default);
    Task DeleteAsync(TodoItem todo, CancellationToken ct = default);
}