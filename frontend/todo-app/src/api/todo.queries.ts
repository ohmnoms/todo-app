import {
  useQuery,
  useMutation,
  useQueryClient,
} from '@tanstack/vue-query';

import {
  getTodos,
  createTodo,
  updateTodo,
  deleteTodo
} from './todo.service';
import type {
    UpdateTodoRequest,
} from '@/models/update-todo-request';
import type {
    CreateTodoRequest,
} from '@/models/create-todo-request';

export function useTodos() {
  return useQuery({
    queryKey: ['todos'],
    queryFn: getTodos,
  });
}

export function useCreateTodo() {
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (payload: CreateTodoRequest) => createTodo(payload),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['todos'] });
    }
  });
}

export function useUpdateTodo() {
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (payload: UpdateTodoRequest) => updateTodo(payload),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['todos'] });
    }
  });
}

export function useDeleteTodo() {
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => deleteTodo(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['todos'] });
    }
  });
}
