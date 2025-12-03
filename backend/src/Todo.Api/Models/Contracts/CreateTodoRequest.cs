namespace Todo.Api.Models.Contracts;
public record CreateTodoRequest
{
    public string Title { get; init; } = string.Empty;
}