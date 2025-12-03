using Microsoft.EntityFrameworkCore;
using Todo.Api.Models.Domain;

namespace Todo.Api.Persistence;

public class TodoRepository(TodoDbContext db) : ITodoRepository
{
    private readonly TodoDbContext _db = db;

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default)
    {
        var todos = await _db
            .TodoItems
            .OrderBy(t => t.CreatedDate)
            .ToListAsync(ct);

        return todos;
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var todo = await _db
            .TodoItems
            .FirstOrDefaultAsync(t => t.Id == id, ct);
        return todo;
    }

    public async Task<TodoItem> AddAsync(TodoItem todo, CancellationToken ct = default)
    {
        var entry = await _db.TodoItems.AddAsync(todo, ct);
        await _db.SaveChangesAsync(ct);
        return entry.Entity;
    }

    public async Task UpdateAsync(TodoItem todo, CancellationToken ct = default)
    {
        _db.TodoItems.Update(todo);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TodoItem todo, CancellationToken ct = default)
    {
        _db.TodoItems.Remove(todo);
        await _db.SaveChangesAsync(ct);
    }
}