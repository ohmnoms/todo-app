using Moq;
using Todo.Api.Models.Domain;
using Todo.Api.Services;
using Todo.Api.Persistence;
using Todo.Api.Models.Contracts;

namespace Todo.Api.Tests;

public class TodoItemsServiceTests
{
    // Dependencies
    private readonly Mock<ITodoItemsRepository> repoMock;

    // System Under Test
    private readonly TodoItemsService _sut;

    public TodoItemsServiceTests()
    {
        repoMock = new Mock<ITodoItemsRepository>();
        _sut = new TodoItemsService(repoMock.Object);
    }
    
    [Fact]
    public async Task CreateAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var createRequest = new CreateTodoRequest
        {
            Title = "New todo"
        };

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        repoMock.Verify(r => r.AddAsync(It.Is<TodoItem>(t => t.Title == "New todo")), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedTodoItem()
    {
        // Arrange
        var createRequest = new CreateTodoRequest
        {
            Title = "New todo"
        };

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        Assert.Equal(createRequest.Title, result.Title);
        Assert.False(result.IsCompleted);
    }

    [Fact]
    public async Task GetByIdAsync_TodoFound_ReturnsTodoItem()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();
        var todoItem = new TodoItem
        {
            Id = todoId,
            Title = "Existing todo",
            IsCompleted = false,
            CreatedBy = createdBy,
        };
        var createdByRequest = new GetTodoRequest
        {
            CreatedBy = createdBy
        };

        repoMock.Setup(r => r.GetByIdAsync(todoId, createdByRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoItem);

        // Act
        var result = await _sut.GetByIdAsync(todoId, createdByRequest);

        // Assert
        Assert.Equal(todoItem, result);
    }

    [Fact]
    public async Task GetByIdAsync_TodoNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var createdByRequest = new GetTodoRequest
        {
            CreatedBy = Guid.NewGuid()
        };
        repoMock.Setup(r => r.GetByIdAsync(nonExistentId, createdByRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetByIdAsync(nonExistentId, createdByRequest));
    }
}
