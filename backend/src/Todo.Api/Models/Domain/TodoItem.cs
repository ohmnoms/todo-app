namespace Todo.Api.Models.Domain;
public record TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? CompletedDate { get; set; }
}