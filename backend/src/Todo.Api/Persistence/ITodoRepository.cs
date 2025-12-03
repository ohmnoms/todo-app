using Todo.Api.Models.Domain;

namespace Todo.Api.Persistence;

public interface ITodoRepository
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default);
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TodoItem> AddAsync(TodoItem todo, CancellationToken ct = default);
    Task UpdateAsync(TodoItem todo, CancellationToken ct = default);
    Task DeleteAsync(TodoItem todo, CancellationToken ct = default);
}