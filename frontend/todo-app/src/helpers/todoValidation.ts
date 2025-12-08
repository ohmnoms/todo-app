// Constants shared between validation functions.  Keep these in sync
// with server side validation logic.
export const maxTitleLength = 240;

/** Validate a todo title.  Returns a human friendly error message if
 * invalid, otherwise returns `null`.
 */
export function validateTodoTitle(value: string): string | null {
  const trimmed = value.trim();
  if (!trimmed) return 'Todo text is required.';
  if (trimmed.length > maxTitleLength) {
    return `Todo text must be ${maxTitleLength} characters or fewer.`;
  }
  return null;
}

/** Validate a due date.  Returns an error message if the date is in the
 * past, otherwise returns `null`.  Accepts ISO date strings or any
 * value parseable by the Date constructor.  An empty value or
 * undefined is considered valid because the field is optional.
 */
export function validateDueDate(value?: string | null): string | null {
  if (!value) return null;
  // Append midnight so that Date parses as local time rather than UTC.
  const dateOnly = new Date(value + 'T00:00');
  // If the date cannot be parsed return null (let the browser catch it).
  if (isNaN(dateOnly.getTime())) return null;
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  if (dateOnly < today) {
    return 'Due date cannot be in the past.';
  }
  return null;
}

/** Validate a due time. Here for parity, no validation rules currently.
 */
// eslint-disable-next-line @typescript-eslint/no-unused-vars
export function validateDueTime(_value?: string | null): string | null {
  return null;
}