import type { CreateTodoRequest } from '@/models/create-todo-request';
import type { UpdateTodoRequest } from '@/models/update-todo-request';
import type { TodoItem } from '../models/todo-item';
import { httpClient } from '../api/httpClient';

export const todoService = {
	getTodos() {
		return httpClient.get<TodoItem[]>('/todos');
	},
	createTodo(payload: CreateTodoRequest) {
		return httpClient.post<TodoItem>('/todos', payload);
	},
	updateTodo(id: string, payload: UpdateTodoRequest) {
		return httpClient.put<TodoItem>(`/todos/${id}`, payload);
	},
	deleteTodo(id: string) {
		return httpClient.delete(`/todos/${id}`);
	},
};
