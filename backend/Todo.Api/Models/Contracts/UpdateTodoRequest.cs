namespace Todo.Api.Models.Contracts;

/// <summary>
/// Request model for updating an existing Todo item.
/// </summary>
public record UpdateTodoRequest 
{
    /// <summary>
    /// The title of the Todo item.
    /// </summary>
    public string Title { get; init; } = string.Empty;
    /// <summary>
    /// Indicates whether the Todo item is completed.
    /// </summary>
    public bool IsCompleted { get; init; }
    /// <summary>
    /// The date and time when the Todo item was completed.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; set; }
    /// <summary>
    /// The date on which the Todo item is due.
    /// </summary>
    public DateOnly? DueDate { get; set; }
    /// <summary>
    /// The time of day at which the Todo item is due.
    /// </summary>
    public TimeOnly? DueTime { get; set; }
    /// <summary>
    /// The identifier of the user who created the Todo item.
    /// </summary>
    public Guid CreatedBy { get; set; }
}