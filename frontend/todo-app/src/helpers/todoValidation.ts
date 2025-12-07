export const maxTitleLength = 240;

export function validateTodoTitle(value: string): string | null {
  const trimmed = value.trim();

  if (!trimmed) return 'Todo text is required.';
  if (trimmed.length > maxTitleLength) {
    return `Todo text must be ${maxTitleLength} characters or fewer.`;
  }
  return null;
}
