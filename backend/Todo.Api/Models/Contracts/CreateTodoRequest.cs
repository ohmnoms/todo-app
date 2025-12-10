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
    
    /// <summary>
    /// Optional date on which the Todo item is due. Leave null for no due date.
    /// </summary>
    public DateOnly? DueDate { get; init; }

    /// <summary>
    /// Optional time of day at which the Todo item is due. Leave null for no due time.
    /// </summary>
    public TimeOnly? DueTime { get; init; }
    /// <summary>
    /// The identifier of the user who created the Todo item.
    /// </summary>
    public Guid CreatedBy { get; set; }
}