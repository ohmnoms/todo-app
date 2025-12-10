namespace Todo.Api.Models.Contracts;

public record GetTodoRequest
{
    /// <summary>
    /// Optional filter to get Todo items created by a specific user/device.
    /// NOTE: This is currently enforced for security reasons, so you cannot access anyone else's Todo items.
    /// Left as optional here for future extensibility.
    /// </summary>
    public Guid? CreatedBy { get; init; }
    /// <summary>
    /// Optional filter to include completed Todo items.
    /// </summary>
    public bool? IsCompleted { get; init; }
}
