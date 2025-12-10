import { todoService } from '@/services/todo.service';
import { useErrorHandler } from '@/composables/useErrorHandler';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import type { TodoItem } from '@/models/todo-item';
import { computed, watch } from 'vue';
import type { UpdateTodoRequest } from '@/models/update-todo-request';
import type { CreateTodoRequest } from '@/models/create-todo-request';
import { useDeviceId } from '@/composables/useDeviceId';
import type { GetTodoRequest } from '@/models/get-todo-request';
import { useTodoFilters } from './useTodoFilters';

export function useTodos() {
  const queryClient = useQueryClient();
  const { handleError } = useErrorHandler();
  const deviceId = useDeviceId();
  const { hideCompleted } = useTodoFilters();

  // READ
  const todosQuery = useQuery<TodoItem[]>({
    queryKey: ['todos', { createdBy: deviceId }],
    queryFn: async () => {
      const payload: GetTodoRequest = {
        createdBy: deviceId,
      };

      if (hideCompleted.value) payload.isCompleted = false;

      try {
        return await todoService.getTodos(payload);
      } catch (err) {
        handleError(err, 'Failed to load todos');
        throw err;
      }
    },
  });

  const todos = computed<TodoItem[]>(() => todosQuery.data.value ?? []);
  watch(hideCompleted, () => {
    queryClient.invalidateQueries({ queryKey: ['todos'] });
  });

  // READ BY ID
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const todoByIdQuery = (id: string, payload: GetTodoRequest) => useQuery({
    queryKey: ['todo', id, payload],
    queryFn: () => todoService.getTodoById(id, payload),
    enabled: !!id,
  });

  // CREATE
  const createTodo = useMutation({
    mutationFn: (payload: CreateTodoRequest) =>
      todoService.createTodo(payload),
    onError: (err) => handleError(err, 'Failed to create todo'),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['todos'] }),
  });

  // UPDATE
  const updateTodo = useMutation({
    mutationFn: ({ id, patch }: { id: string; patch: Partial<TodoItem> }) => {
      const payload: UpdateTodoRequest = {
        id,
        title: patch.title,
        isCompleted: patch.isCompleted,
        dueDate: patch.dueDate ?? null,
        dueTime: patch.dueTime ?? null,
        completedDate: patch.completedDate ?? null,
        createdBy: deviceId,
      };

      return todoService.updateTodo(id, payload);
    },
    onError: (err) => handleError(err, 'Failed to update todo'),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['todos'] }),
  });

  // DELETE
  const deleteTodo = useMutation({
    mutationFn: (id: string) => todoService.deleteTodo(id),
    onError: (err) => handleError(err, 'Failed to delete todo'),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['todos'] }),
  });

  return {
    todosQuery,
	  todos,
    createTodo,
    updateTodo,
    deleteTodo,
  };
}
