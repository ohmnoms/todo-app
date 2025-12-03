import type { TodoItem } from "@/models/todo-item";
import type { UpdateTodoRequest } from "@/models/update-todo-request";
import api from "./client";
import type { CreateTodoRequest } from "@/models/create-todo-request";

export async function getTodos(): Promise<TodoItem[]> {
  const res = await api.get<TodoItem[]>('/todo');
  return res.data;
}

export async function getTodoById(id: string): Promise<TodoItem> {
  const res = await api.get<TodoItem>(`/todo/${id}`);
  return res.data;
}

export async function createTodo(
  payload: CreateTodoRequest
): Promise<TodoItem> {
  const res = await api.post<TodoItem>('/todo', payload);
  return res.data;
}

export async function updateTodo(
  payload: UpdateTodoRequest
): Promise<TodoItem> {
  const res = await api.put<TodoItem>('/todo', payload);
  return res.data;
}

export async function deleteTodo(id: string): Promise<void> {
  await api.delete(`/todo/${id}`);
}
