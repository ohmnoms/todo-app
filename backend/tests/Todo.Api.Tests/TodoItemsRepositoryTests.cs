using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Models.Domain;
using Todo.Api.Persistence;
using Xunit;

namespace Todo.Api.Tests;

public class TodoItemsRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly TodoDbContext _db;

    public TodoItemsRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open(); // IMPORTANT: keep open for the lifetime of the context

        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new TodoDbContext(options);
        _db.Database.EnsureCreated();

        // seed data
        _db.TodoItems.Add(new TodoItem { Id = new Guid(), Title = "Existing", IsCompleted = false });
        _db.SaveChanges();
    }

    [Fact]
    public async Task GetAll_ReturnsSeededTodos()
    {
        var repo = new TodoItemsRepository(_db);

        var todos = await repo.GetAllAsync();

        Assert.Single(todos);
        Assert.Equal("Existing", todos[0].Title);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}
