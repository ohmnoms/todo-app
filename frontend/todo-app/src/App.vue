<script setup lang="ts">
import TodoForm from './components/TodoForm.vue';
import TodoList from './components/TodoList.vue';
import { useTodos } from './composables/use-todos';

const { todos, loading, error, addTodo, toggleTodo, removeTodo } = useTodos();
</script>

<template>
  <div class="app-shell">
    <div class="card bg-base-100 shadow-lg">
      <div class="card-body space-y-4">
        <h2 class="card-title">Do It</h2>

        <TodoForm @submit="addTodo" />
        
        <div v-if="loading">Loading…</div>
        <div v-else-if="error">Error: {{ String(error) }}</div>
        <TodoList
          :todos="todos"
          @toggle-complete="toggleTodo"
          @delete="removeTodo"
        />
      </div>
    </div>
  </div>
</template>
