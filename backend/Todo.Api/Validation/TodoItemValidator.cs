

using Todo.Api.Common;
using Todo.Api.Models.Domain;

namespace Todo.Api.Validation;
public static class TodoItemValidator
{
    public static async Task ValidateTitleAsync(
        string title,
        CancellationToken ct = default)
    {
        var errors = new Dictionary<string, List<string>>();

        // Required
        if (string.IsNullOrWhiteSpace(title))
        {
            AddError(errors, "title", "Title is required.");
        }

        // Length
        if (!string.IsNullOrWhiteSpace(title) && title.Length > TodoItem.MaxTitleLength)
        {
            AddError(errors, "title", $"Title cannot exceed {TodoItem.MaxTitleLength} characters.");
        }
        
        if (errors.Count > 0)
        {
            throw new ValidationException(
                errors.ToDictionary(error => error.Key, error => error.Value.ToArray()));
        }
    }
    
    /// <summary>
    /// Validates a due date value. A due date cannot be in the past.
    /// </summary>
    public static async Task ValidateDueDateAsync(DateOnly? dueDate, CancellationToken ct = default)
    {
        // no-op if no due date provided
        if (dueDate is null)
        {
            return;
        }
        var errors = new Dictionary<string, List<string>>();
        // Compare using current date in system time zone
        var today = DateOnly.FromDateTime(DateTimeOffset.Now.Date);
        if (dueDate.Value < today)
        {
            AddError(errors, "dueDate", "Due date cannot be in the past.");
        }
        if (errors.Count > 0)
        {
            throw new ValidationException(errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));
        }
    }

    /// <summary>
    /// Validates a due time value. No specific constraints yet, but reserved for future logic.
    /// </summary>
    public static async Task ValidateDueTimeAsync(TimeOnly? dueTime, CancellationToken ct = default)
    {
        // no-op if no due time provided
        if (dueTime is null)
        {
            return;
        }
        var errors = new Dictionary<string, List<string>>();
        // Compare using current date in system time zone
        var currentTime = TimeOnly.FromDateTime(DateTimeOffset.Now.Date);
        if (dueTime.Value < currentTime)
        {
            AddError(errors, "dueTime", "Due time cannot be in the past.");
        }
        if (errors.Count > 0)
        {
            throw new ValidationException(errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));
        }
    }

    private static void AddError(
        IDictionary<string, List<string>> errors,
        string key,
        string errorMessage)
    {
        if (!errors.TryGetValue(key, out List<string>? value))
        {
            value = [];
            errors[key] = value;
        }

        value.Add(errorMessage);
    }
}
