namespace Todo.Api.Models.Contracts;
public record UpdateTodoRequest 
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
}