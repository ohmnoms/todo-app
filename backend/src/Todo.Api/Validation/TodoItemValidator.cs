

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
