using Microsoft.EntityFrameworkCore;
using Todo.Api.Models.Domain;

namespace Todo.Api.Persistence;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
