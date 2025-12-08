namespace Todo.Api.Models.Domain;

/// <summary>
/// Domain model representing a Todo item.
/// </summary>
public record TodoItem
{
    /// <summary>
    /// Maximum length for the title of a Todo item.
    /// </summary>
    public const int MaxTitleLength = 240;
    /// <summary>
    /// The unique identifier of the Todo item.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The title of the Todo item.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Indicates whether the Todo item is completed.
    /// </summary>
    public bool IsCompleted { get; set; }
    /// <summary>
    /// The date and time when the Todo item was created.
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }
    /// <summary>
    /// The date and time when the Todo item was completed.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; set; }
}