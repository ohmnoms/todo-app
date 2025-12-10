export interface CreateTodoRequest {
  title: string;
  dueDate?: string | null;
  dueTime?: string | null;
  completedDate?: string | null; // As of now null on creation
  createdBy: string;
}