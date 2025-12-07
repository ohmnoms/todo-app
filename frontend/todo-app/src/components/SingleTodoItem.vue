
<script setup lang="ts">
import type { TodoItem } from '@/models/todo-item';

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const props = defineProps<{
  todo: TodoItem;
}>();

const emit = defineEmits<{
  (e: 'toggle-complete'): void;
  (e: 'delete'): void;
}>();
</script>

<template>
  <li class="flex items-center gap-2" data-testid="todo-item">
    <input
      type="checkbox"
      data-testid="todo-toggle"
      class="checkbox checkbox-sm"
      :checked="todo.isCompleted"
      @change="emit('toggle-complete')"
    />
    <span
      data-testid="todo-title-text"
      :class="{
        'line-through text-base-content/60': todo.isCompleted,
      }"
    >
      {{ todo.title }}
    </span>

    <button
      type="button"
      class="btn btn-ghost btn-xs text-error ml-auto"
      data-testid="todo-delete-button"
      @click="emit('delete')"
    >
      ✕
    </button>
  </li>
</template>
