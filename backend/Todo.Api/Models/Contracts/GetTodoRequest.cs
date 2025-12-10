namespace Todo.Api.Models.Contracts;

public record GetTodoRequest {
    /// <summary>
    /// Optional filter to get Todo items created by a specific user/device.
    /// </summary>
    public Guid? CreatedBy { get; init; }
    /// <summary>
    /// Optional filter to include completed Todo items.
    /// </summary>
    public bool? IsCompleted { get; init; }
}
