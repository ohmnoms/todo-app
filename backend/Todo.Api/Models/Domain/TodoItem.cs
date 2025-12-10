using Todo.Api.Models.Contracts;

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
    /// <summary>
    /// The date on which the Todo item is due. Date-only to allow setting
    /// a calendar date without requiring a time of day.
    /// </summary>
    public DateOnly? DueDate { get; set; }
    /// <summary>
    /// The time of day at which the Todo item is due. Time-only to allow
    /// fine‑grained control over deadlines without affecting the calendar date.
    /// </summary>
    public TimeOnly? DueTime { get; set; }
    /// <summary>
    /// The identifier of the user who created the Todo item.
    /// </summary>
    public Guid CreatedBy { get; set; }

    public TodoItemResponseDTO ToDTO()
    {
        return new TodoItemResponseDTO
        {
            Id = Id,
            Title = Title,
            IsCompleted = IsCompleted,
            CreatedDate = CreatedDate,
            CompletedDate = CompletedDate,
            DueDate = DueDate,
            DueTime = DueTime,
            CreatedBy = CreatedBy
        };
    }
}