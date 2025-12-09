export interface UpdateTodoRequest {
  id: string;
  title?: string;
  isCompleted?: boolean;
  dueDate?: string | null;
  dueTime?: string | null;
  completedDate?: string | null;
}