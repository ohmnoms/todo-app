import { ref, shallowRef, onMounted } from 'vue';
import { todoService } from '@/services/todo.service';
import type { TodoItem } from '@/models/todo-item';

export function useTodos() {
	const todos = shallowRef<TodoItem[]>([]);
	const loading = ref(false);
	const error = ref<unknown>(null);

	async function loadTodos() {
		loading.value = true;
		error.value = null;
		try {
			todos.value = await todoService.getTodos();
		} catch (err) {
			error.value = err;
		} finally {
			loading.value = false;
		}
	}

	async function addTodo(title: string) {
		const created = await todoService.createTodo({ title });
		todos.value = [...todos.value, created];
	}

	async function toggleTodo(todo: TodoItem) {
		const updated = await todoService.updateTodo(todo.id, {
			isCompleted: !todo.isCompleted,
		});
		todos.value = todos.value.map((t) => (t.id === todo.id ? updated : t));
	}

	async function removeTodo(id: string) {
		await todoService.deleteTodo(id);
		todos.value = todos.value.filter((t) => t.id !== id);
	}

	onMounted(loadTodos);

	return {
		todos,
		loading,
		error,
		loadTodos,
		addTodo,
		toggleTodo,
		removeTodo,
	};
}
