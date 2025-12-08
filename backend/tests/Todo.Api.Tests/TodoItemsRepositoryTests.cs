using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Models.Domain;
using Todo.Api.Persistence;

namespace Todo.Api.Tests;

public class TodoItemsRepositoryTests : IDisposable
{
    // Dependencies
    private readonly SqliteConnection _connection;
    private readonly TodoDbContext _db;
    private readonly Guid _existingTodoId;

    // System Under Test
    private readonly TodoItemsRepository _sut;

    public TodoItemsRepositoryTests()
    {

        // Create test connection
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open(); // keep open for the lifetime of the context

        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new TodoDbContext(options);
        _db.Database.EnsureCreated();

        // seed data for tests
        _existingTodoId = Guid.NewGuid();
        _db.TodoItems.Add(new TodoItem { Id = _existingTodoId, Title = "Existing", IsCompleted = false });
        _db.SaveChanges();

        _sut = new TodoItemsRepository(_db);
    }

    [Fact]
    public async Task GetAll_ReturnsSeededTodos()
    {
        // Arrange & Act
        var todos = await _sut.GetAllAsync();

        // Assert
        Assert.Single(todos);
    }

    [Fact]
    public async Task GetById_ReturnsTodo()
    {
        // Arrange & Act
        var todo = await _sut.GetByIdAsync(_existingTodoId);

        // Assert
        Assert.Equal("Existing", todo?.Title);
    }

    [Fact]
    public async Task GetById_TodoNotFound_ReturnsNull()
    {
        // Arrange & Act
        var todo = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(todo);
    }

    [Fact]
    public async Task Add_AddsNewTodo()
    {
        // Arrange
        var newTodo = new TodoItem { Id = Guid.NewGuid(), Title = "New Todo", IsCompleted = false };

        // Act
        await _sut.AddAsync(newTodo);

        // Assert
        var todos = await _sut.GetAllAsync();
        Assert.Equal(2, todos.Count);
        Assert.Contains(todos, t => t.Id == newTodo.Id);
    }

    [Fact]
    public async Task Add_ReturnsNewTodo()
    {
        // Arrange
        var newTodo = new TodoItem { Title = "New Todo", IsCompleted = false };

        // Act
        var result = await _sut.AddAsync(newTodo);

        // Assert
        Assert.Equal(newTodo.Title, result.Title);
    }

    [Fact]
    public async Task Update_UpdatesExistingTodo()
    {
        // Arrange
        var todo = await _sut.GetByIdAsync(_existingTodoId);
        Assert.NotNull(todo);
        todo!.Title = "Updated Title";

        // Act
        await _sut.UpdateAsync(todo);
        var updatedTodo = await _sut.GetByIdAsync(_existingTodoId);

        // Assert
        Assert.Equal("Updated Title", updatedTodo?.Title);
    }

    [Fact]
    public async Task Delete_RemovesTodo()
    {
        // Arrange
        var toDelete = await _sut.GetByIdAsync(_existingTodoId);
        Assert.NotNull(toDelete);

        // Act
        await _sut.DeleteAsync(toDelete);

        // Assert
        toDelete = await _sut.GetByIdAsync(_existingTodoId);
        Assert.Null(toDelete);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}
