/**
 * Represents a single todo item.
 * @module models/todo-item
 */
export interface TodoItem {
  /** Unique identifier for the todo item. */
  id: string;
  /** Title or description of the todo item. */
  title: string;
  /** Whether the todo item is completed. */
  isCompleted: boolean;
  /** Date and time when the todo item was created. */
  createdDate: string;
  /**
   * When a todo is completed the API returns the timestamp at which it
   * was completed.  This value is optional because uncompleted items
   * don't have a completion date.  Consumers should treat `null` as
   * “not completed yet”.
   */
  completedDate?: string | null;
  /**
   * Optional calendar date for when the task is due.  Stored as an ISO
   * 8601 date string (YYYY‑MM‑DD) or `null` if no date has been
   * assigned.  A date in the past will be rejected by the API.
   */
  dueDate?: string | null;
  /**
   * Optional time on the due date when the task should be completed.
   * Stored as an ISO 8601 time string (HH:mm or HH:mm:ss) or `null` if
   * no time has been assigned.  If present, a `dueDate` should also be
   * provided.
   */
  dueTime?: string | null;
  /** Identifier for the creator of the todo item. */
  createdBy: string;
}