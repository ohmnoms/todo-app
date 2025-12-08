
namespace Todo.Api.Models.Contracts;

/// <summary>
/// Data transfer object representing a Todo item in API responses.
/// </summary>
public record TodoItemResponseDTO
{
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