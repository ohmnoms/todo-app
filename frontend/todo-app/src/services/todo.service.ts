import type { CreateTodoRequest } from '@/models/create-todo-request';
import type { UpdateTodoRequest } from '@/models/update-todo-request';
import type { TodoItem } from '../models/todo-item';
import { httpClient } from '../api/httpClient';
import type { GetTodoRequest } from '@/models/get-todo-request';

export const todoService = {
	getTodos(payload: GetTodoRequest) {
		return httpClient.get<TodoItem[]>('/todos', payload );
	},
	getTodoById(id: string, payload: GetTodoRequest) {
		return httpClient.get<TodoItem>(`/todos/${id}`, payload);
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
