<script setup lang="ts">
import type { TodoItem } from '@/models/todo-item';
import SingleTodoItem from './SingleTodoItem.vue';

const props = defineProps<{
  todos: TodoItem[];
}>();

const emit = defineEmits<{
  (e: 'toggle-complete', id: string): void;
  (e: 'delete', id: string): void;
}>();
</script>

<template>
  <div v-if="!todos.length" class="alert alert-info">
    <span>No todos yet. Add your first one above.</span>
  </div>

  <ul v-else class="space-y-2">
    <SingleTodoItem
      v-for="todo in todos"
      :key="todo.id"
      :todo="todo"
      @toggle-complete="emit('toggle-complete', todo.id)"
      @delete="emit('delete', todo.id)"
    />
  </ul>
</template>
