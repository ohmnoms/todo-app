<script setup lang="ts">
import type { TodoItem } from '@/models/todo-item';
import SingleTodoItem from './SingleTodoItem.vue';

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const props = defineProps<{
  todos: TodoItem[];
}>();

const emit = defineEmits<{
  (e: 'toggle-complete', id: string): void;
  (e: 'delete', id: string): void;
  (e: 'update', id: string, payload: Partial<TodoItem>): void;
}>();
</script>

<template>
  <div v-if="!todos.length" class="alert alert-info">
    <span>No todos yet. Add one above.</span>
  </div>

  <ul v-else class="space-y-2" data-testid="todo-list">
    <SingleTodoItem
      v-for="todo in todos"
      :key="todo.id"
      :todo="todo"
      @toggle-complete="emit('toggle-complete', todo.id)"
      @delete="emit('delete', todo.id)"
      @update="(payload) => emit('update', todo.id, payload)"
    />
  </ul>
</template>
