using Microsoft.AspNetCore.Mvc;
using Todo.Api.Models.Contracts;
using Todo.Api.Models.Domain;
using Todo.Api.Services;

namespace Todo.Api.Controllers;

/// <summary>
/// Controller for managing Todo items.
/// </summary>
/// <param name="todoService"></param>
[ApiController]
[Route("api/[controller]")]
public class TodosController(ITodoItemsService todoService) : ControllerBase
{
    private readonly ITodoItemsService _todoService = todoService;

    /// <summary>
    /// Gets all Todo items.
    /// </summary>
    /// <returns>A list of todos</returns>
    /// <response code="200">Returns the list of Todo items</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTodos()
    {
        var todos = await _todoService.GetAllAsync();
        return Ok(todos);
    }

    /// <summary>
    /// Gets a Todo item by its ID.
    /// </summary>
    /// <param name="id">The ID of the Todo item.</param>
    /// <returns>The requested Todo item.</returns>
    /// <response code="200">Returns the requested Todo item</response>
    /// <response code="404">If the Todo item is not found</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTodoById(Guid id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        return Ok(todo);
    }

    /// <summary>
    /// Creates a new Todo item.
    /// </summary>
    /// <param name="request">The Todo item to create.</param>
    /// <returns>The created Todo item.</returns>
    /// <response code="201">Returns the created Todo item</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var todo = await _todoService.CreateAsync(request);
        return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
    }

    /// <summary>
    /// Updates an existing Todo item.
    /// </summary>
    /// <param name="id">The ID of the Todo item to update.</param>
    /// <param name="request">The updated Todo item.</param>
    /// <returns>The updated Todo item.</returns>
    /// <response code="200">Returns the updated Todo item</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the Todo item is not found</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] UpdateTodoRequest request)
    {
        var todo = await _todoService.UpdateAsync(id, request);
        return Ok(todo);
    }

    /// <summary>
    /// Deletes a Todo item by its ID.
    /// </summary>
    /// <param name="id">The ID of the Todo item to delete.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the Todo item was successfully deleted</response>
    /// <response code="404">If the Todo item is not found</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        await _todoService.DeleteAsync(id);
        return NoContent();
    }
}