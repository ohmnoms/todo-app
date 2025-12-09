<script setup lang="ts">
import { computed } from 'vue';
import TodoForm from '@/components/TodoForm.vue';
import TodoList from '@/components/TodoList.vue';
import { useTodos } from '@/composables/useTodos';
import TodoItemSkeletonLoader from '@/components/TodoItemSkeletonLoader.vue';
import type { TodoItem } from '@/models/todo-item';
import type { CreateTodoRequest } from '@/models/create-todo-request';
import type { UpdateTodoRequest } from '@/models/update-todo-request';

const { todosQuery, todos, createTodo, updateTodo, deleteTodo } = useTodos();

const isLoading = computed(() => todosQuery.isPending.value);
</script>

<template>
  <div class="justify-center text-center">
    <h1 class="text-2xl font-bold mb-4">Simple To Do</h1>
    <h3 class="text-lg mb-6 text-gray-600">Keep it simple</h3>
  </div>
  <section class="space-y-4">
    <TodoForm
      :busy="createTodo.isPending.value"
      @submit="(payload: CreateTodoRequest) => createTodo.mutate(payload)"
    />

    <TodoItemSkeletonLoader v-if="isLoading" data-testid="todo-skeleton" />

    <TodoList
      v-else
      :todos="todos"
      @toggle-complete="(id: string) => {
        const todo = todos.find((t: TodoItem) => t.id === id);
        if (todo) {
          updateTodo.mutate({ id: todo.id, patch: { 
            ...todo, 
            isCompleted: !todo.isCompleted,
            completedDate: !todo.isCompleted 
              ? new Date().toISOString() : null
          } as UpdateTodoRequest });
        }
      }"
      @delete="(id: string) => deleteTodo.mutate(id)"
      @update="(id: string, patch: UpdateTodoRequest) => updateTodo.mutate({ id, patch })"
    />
  </section>
</template>
