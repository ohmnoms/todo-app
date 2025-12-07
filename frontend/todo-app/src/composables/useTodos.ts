import { todoService } from '@/services/todo.service';
import { useErrorHandler } from '@/composables/useErrorHandler';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import type { TodoItem } from '@/models/todo-item';
import { computed } from 'vue';

export function useTodos() {
  const queryClient = useQueryClient();
  const { handleError } = useErrorHandler();

  // READ
  const todosQuery = useQuery({
    queryKey: ['todos'],
    queryFn: () => todoService.getTodos(),
  });

  const todos = computed<TodoItem[]>(() => todosQuery.data.value ?? []);

  // CREATE
  const createTodo = useMutation({
    mutationFn: (title: string) => todoService.createTodo({ title }),
    onError: (err) => handleError(err, 'Failed to create todo'),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['todos'] }),
  });

  // UPDATE
  const updateTodo = useMutation({
    mutationFn: (payload: { id: string; patch: Partial<TodoItem> }) =>
      todoService.updateTodo(payload.id, payload.patch),
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
