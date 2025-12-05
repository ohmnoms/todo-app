namespace Todo.Api.Models.Contracts;

/// <summary>
/// Request model for creating a new Todo item.
/// </summary>
public record CreateTodoRequest
{
    /// <summary>
    /// The title of the Todo item.
    /// </summary>
    public string Title { get; init; } = string.Empty;
}