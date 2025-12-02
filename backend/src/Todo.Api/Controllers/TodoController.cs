using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    // TODO: Add service DI (repository pattern)
    public TodoController()
    {
    }

    [HttpGet]
    public IActionResult GetTodos()
    {
        // TODO: Implement method to get all todos
        return Ok();
    }

    [HttpPost]
    [Route("create")]
    public IActionResult CreateTodo([FromBody] CreateTodoRequest request)
    {
        // TODO: Implement method to create a new todo
        return Ok();
    }

    [HttpPut]
    [Route("update")]
    public IActionResult UpdateTodo([FromBody] UpdateTodoRequest request)
    {
        // TODO: Implement method to update an existing todo
        return Ok();
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult DeleteTodo(Guid id)
    {
        // TODO: Implement method to delete a todo by id
        return Ok();
    }
}