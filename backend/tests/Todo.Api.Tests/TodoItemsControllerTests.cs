using Moq;

using Microsoft.AspNetCore.Mvc;
using Todo.Api.Controllers;
using Todo.Api.Models.Domain;
using Todo.Api.Services;
using Todo.Api.Models.Contracts;

namespace Todo.Api.Tests;

public class TodosControllerTests
{
    // Dependencies
    private readonly Mock<ITodoItemsService> serviceMock;
    
    // System Under Test
    private readonly TodosController _sut;

    public TodosControllerTests()
    {
        serviceMock = new Mock<ITodoItemsService>();
        _sut = new TodosController(serviceMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOkResult()
    {
        // Arrange
        var todos = new List<TodoItem> { new() { Id = Guid.NewGuid(), Title = "Existing" } };
        serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(todos);

        // Act
        var result = await _sut.GetTodos();
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Get_ReturnsTodosFromService()
    {
        // Arrange
        var todos = new List<TodoItem> { 
            new() { Id = Guid.NewGuid(), Title = "Existing 1" },
            new() { Id = Guid.NewGuid(), Title = "Existing 2" } 
        };
        serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(todos);

        // Act
        var result = await _sut.GetTodos() as OkObjectResult;
        
        // Assert
        var returnedTodos = Assert.IsType<List<TodoItem>>(result?.Value);
        Assert.Equal(2, returnedTodos.Count);
        Assert.Equal("Existing 1", returnedTodos[0].Title);
        Assert.Equal("Existing 2", returnedTodos[1].Title);
    }

    [Fact]
    public async Task GetById_TodoFound_ReturnsOkResult()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var todo = new TodoItem { Id = todoId, Title = "Existing" };
        serviceMock.Setup(s => s.GetByIdAsync(todoId)).ReturnsAsync(todo);

        // Act
        var result = await _sut.GetTodoById(todoId);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_TodoFound_ReturnsTodoFromService()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var todo = new TodoItem { Id = todoId, Title = "Existing" };
        serviceMock.Setup(s => s.GetByIdAsync(todoId)).ReturnsAsync(todo);

        // Act
        var result = await _sut.GetTodoById(todoId) as OkObjectResult;
        
        // Assert
        var returnedTodo = Assert.IsType<TodoItem>(result?.Value);
        Assert.Equal("Existing", returnedTodo.Title);
    }

    [Fact]
    public async Task GetById_TodoNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        serviceMock.Setup(s => s.GetByIdAsync(todoId)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _sut.GetTodoById(todoId);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_CallsServiceCreateAsync()
    {
        // Arrange
        var createRequest = new CreateTodoRequest
        {
            Title = "New todo"
        };

        // Act
        var result = await _sut.CreateTodo(createRequest);

        // Assert
        serviceMock.Verify(s => s.CreateAsync(It.Is<CreateTodoRequest>(r => r.Title == "New todo")), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var createRequest = new CreateTodoRequest
        {
            Title = "New todo"
        };

        var created = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "New todo"
        };

        serviceMock.Setup(s => s.CreateAsync(It.IsAny<CreateTodoRequest>())).ReturnsAsync(created);

        // Act
        var result = await _sut.CreateTodo(createRequest);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_sut.GetTodoById), createdAtActionResult.ActionName);
        var returnedTodo = Assert.IsType<TodoItem>(createdAtActionResult.Value);
        Assert.Equal("New todo", returnedTodo.Title);
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var createRequest = new CreateTodoRequest
        {
            Title = "" // Invalid: Title is required
        };
        _sut.ModelState.AddModelError("Title", "The Title field is required.");

        // Act
        var result = await _sut.CreateTodo(createRequest);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_CallsServiceUpdateAsync()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var updateRequest = new UpdateTodoRequest
        {
            Title = "Updated todo",
            IsCompleted = true
        };

        // Act
        var result = await _sut.UpdateTodo(todoId, updateRequest);

        // Assert
        serviceMock.Verify(s => s.UpdateAsync(
            It.Is<Guid>(id => id == todoId),
            It.Is<UpdateTodoRequest>(r => r.Title == "Updated todo" && r.IsCompleted)), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsOkResult()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var updateRequest = new UpdateTodoRequest
        {
            Title = "Updated todo",
            IsCompleted = true
        };

        var updatedTodo = new TodoItem
        {
            Id = todoId,
            Title = "Updated todo",
            IsCompleted = true
        };

        serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateTodoRequest>()))
            .ReturnsAsync(updatedTodo);

        // Act
        var result = await _sut.UpdateTodo(todoId, updateRequest);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsUpdatedTodo()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var updateRequest = new UpdateTodoRequest
        {
            Title = "Updated todo",
            IsCompleted = true
        };

        var updatedTodo = new TodoItem
        {
            Id = todoId,
            Title = "Updated todo",
            IsCompleted = true
        };

        serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateTodoRequest>()))
            .ReturnsAsync(updatedTodo);

        // Act
        var result = await _sut.UpdateTodo(todoId, updateRequest) as OkObjectResult;

        // Assert
        var returnedTodo = Assert.IsType<TodoItem>(result?.Value);
        Assert.Equal("Updated todo", returnedTodo.Title);
        Assert.True(returnedTodo.IsCompleted);
    }

    [Fact]
    public async Task Delete_CallsServiceDeleteAsync()
    {
        // Arrange
        var todoId = Guid.NewGuid();

        // Act
        var result = await _sut.DeleteTodo(todoId);

        // Assert
        serviceMock.Verify(s => s.DeleteAsync(It.Is<Guid>(id => id == todoId)), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNoContentResult()
    {
        // Arrange
        var todoId = Guid.NewGuid();

        // Act
        var result = await _sut.DeleteTodo(todoId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_TodoNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        serviceMock.Setup(s => s.DeleteAsync(todoId)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _sut.DeleteTodo(todoId);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
