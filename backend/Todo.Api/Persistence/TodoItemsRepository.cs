using Microsoft.EntityFrameworkCore;
using Todo.Api.Models.Contracts;
using Todo.Api.Models.Domain;

namespace Todo.Api.Persistence;

public class TodoItemsRepository(TodoDbContext db) : ITodoItemsRepository
{
    private readonly TodoDbContext _db = db;

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(GetTodoRequest? request = null, CancellationToken ct = default)
    {
        var query = _db.TodoItems.AsQueryable();

        if (request != null)
        {
            if (request.CreatedBy != null)
            {
                query = query.Where(t => t.CreatedBy == request.CreatedBy);
            }
            if (request.IsCompleted != null)
            {
                query = query.Where(t => t.IsCompleted == request.IsCompleted.Value);
            }
        }

        var todos = await query.ToListAsync(ct);

        // Sort by completion status first (incomplete items come first),
        // then by presence of due date/time (items with due dates come first),
        // then by due date, due time and finally by creation date. This ensures that
        // incomplete upcoming tasks appear at the top and completed tasks appear last.
        //
        // Sorting outside the database as a limitation of SQLite's handling of DateTimeOffset
        // and the custom conversions for DateOnly and TimeOnly make it complex to express.
        // For larger datasets, I would consider implementing sorting in the database layer.
        return [.. todos
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate.HasValue ? 0 : 1)
            .ThenBy(t => t.DueDate)
            .ThenBy(t => t.DueTime)
            .ThenBy(t => t.CreatedDate)];
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id, GetTodoRequest? request = null, CancellationToken ct = default)
    {
        var query = _db.TodoItems.AsQueryable();

        if (request != null)
        {
            if (request.CreatedBy != null)
            {
                query = query.Where(t => t.CreatedBy == request.CreatedBy.Value);
            }
        }

        var todos = await query.ToListAsync(ct);
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