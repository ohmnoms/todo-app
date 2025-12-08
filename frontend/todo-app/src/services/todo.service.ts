import { httpClient } from '../api/httpClient';
import type { TodoItem } from '../models/todo-item';

export const todoService = {
	getTodos() {
		return httpClient.get<TodoItem[]>('/todos');
	},
	createTodo(payload: { title: string; dueDate?: string | null; dueTime?: string | null }) {
		return httpClient.post<TodoItem>('/todos', payload);
	},
	updateTodo(id: string, payload: Partial<TodoItem>) {
		return httpClient.put<TodoItem>(`/todos/${id}`, payload);
	},
	deleteTodo(id: string) {
		return httpClient.delete(`/todos/${id}`);
	},
};
