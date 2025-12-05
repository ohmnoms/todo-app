import { httpClient } from '../api/httpClient';
import type { TodoItem } from '../models/todo-item';

export const todoService = {
	getTodos() {
		return httpClient.get<TodoItem[]>('/todos');
	},
	createTodo(payload: Pick<TodoItem, 'title'>) {
		return httpClient.post<TodoItem>('/todos', payload);
	},
	updateTodo(id: string, payload: Partial<TodoItem>) {
		return httpClient.patch<TodoItem>(`/todos/${id}`, payload);
	},
	deleteTodo(id: string) {
		return httpClient.delete<void>(`/todos/${id}`);
	},
};
