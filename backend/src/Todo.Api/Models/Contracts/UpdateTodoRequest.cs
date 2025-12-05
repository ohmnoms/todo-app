namespace Todo.Api.Models.Contracts;

/// <summary>
/// Request model for updating an existing Todo item.
/// </summary>
public record UpdateTodoRequest 
{
    /// <summary>
    /// The ID of the Todo item.
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// The title of the Todo item.
    /// </summary>
    public string Title { get; init; } = string.Empty;
    /// <summary>
    /// Indicates whether the Todo item is completed.
    /// </summary>
    public bool IsCompleted { get; init; }
}