/**
 * Converts a DateTimeOffset string to a local datetime-local string for input fields.
 */
export function toDateTimeLocal(value: string | null | undefined): string | null {
  if (!value) return null;

  const d = new Date(value);
  if (Number.isNaN(d.getTime())) return null;

  const pad = (n: number) => n.toString().padStart(2, '0');

  const year = d.getFullYear();
  const month = pad(d.getMonth() + 1);
  const day = pad(d.getDate());
  const hours = pad(d.getHours());
  const minutes = pad(d.getMinutes());

  // exactly what datetime-local wants
  return `${year}-${month}-${day}T${hours}:${minutes}`;
}

/**
 * Converts a local datetime-local string from input fields to a DateTimeOffset string.
 * Returns null if input is null or empty.
 */
export function fromDateTimeLocal(value: string | null): string | null {
  if (!value) return null;

  const date = new Date(value);
  return date.toISOString(); // "YYYY-MM-DDTHH:mm:ss.sssZ"
}
