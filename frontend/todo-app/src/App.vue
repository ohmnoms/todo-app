<script setup lang="ts">
import { ref } from 'vue';
import TodoForm from './components/TodoForm.vue';
import TodoList from './components/TodoList.vue';
import type { TodoItem } from '@/models/todo-item';

const todos = ref<TodoItem[]>([]);

function addTodo(title: string) {
  const newTodo: TodoItem = {
    id: crypto.randomUUID(), // TEMP
    title,
    isCompleted: false,
  };

  todos.value = [...todos.value, newTodo];
}

function toggleTodoComplete(id: string) {
  todos.value = todos.value.map((t) =>
    t.id === id ? { ...t, isCompleted: !t.isCompleted } : t,
  );
}

function deleteTodo(id: string) {
  todos.value = todos.value.filter((t) => t.id !== id);
}
</script>

<template>
  <div class="app-shell">
    <div class="card bg-base-100 shadow-lg">
      <div class="card-body space-y-4">
        <h2 class="card-title">Do It</h2>

        <TodoForm @submit="addTodo" />

        <TodoList
          :todos="todos"
          @toggle-complete="toggleTodoComplete"
          @delete="deleteTodo"
        />
      </div>
    </div>
  </div>
</template>
