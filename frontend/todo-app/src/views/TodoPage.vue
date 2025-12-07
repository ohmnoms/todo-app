<script setup lang="ts">
import { computed } from 'vue';
import TodoForm from '@/components/TodoForm.vue';
import TodoList from '@/components/TodoList.vue';
import { useTodos } from '@/composables/useTodos';
import TodoItemSkeletonLoader from '@/components/TodoItemSkeletonLoader.vue';
import type { TodoItem } from '@/models/todo-item';

const { todosQuery, todos, createTodo, updateTodo, deleteTodo } = useTodos();

const isLoading = computed(() => todosQuery.isPending.value);
</script>

<template>
  <section class="space-y-4">
    <TodoForm
      :busy="createTodo.isPending.value"
      @submit="(title: string) => createTodo.mutate(title)"
    />

    <!-- Show skeleton loader while loading -->
    <TodoItemSkeletonLoader v-if="isLoading" />

    <!-- Once loaded (or errored with stale data), render list -->
    <TodoList
      v-else
      :todos="todos"
      @toggle-complete="(id: string) => {
        const todo = todos.find((t: TodoItem) => t.id === id);
        if (todo) {
          updateTodo.mutate({ id: todo.id, patch: { ...todo, isCompleted: !todo.isCompleted } });
        }
      }"
      @delete="(id: string) => deleteTodo.mutate(id)"
    />
  </section>
</template>
