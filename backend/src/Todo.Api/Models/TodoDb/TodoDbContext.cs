using Microsoft.EntityFrameworkCore;

namespace Todo.Api.Models.TodoDb;

public interface ITodoDbContext : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<TodoItem> TodoItems { get; }
}


public partial class TodoDbContext : DbContext, ITodoDbContext
{
    public TodoDbContext()
    {
    }

    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TodoItem> TodoItems { get; set; }

}


