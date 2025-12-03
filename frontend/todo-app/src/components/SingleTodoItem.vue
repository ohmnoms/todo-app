
<script setup lang="ts">
import type { TodoItem } from '@/models/todo-item';

const props = defineProps<{
  todo: TodoItem;
}>();

const emit = defineEmits<{
  (e: 'toggle-complete'): void;
  (e: 'delete'): void;
}>();
</script>

<template>
  <li class="flex items-center gap-2">
    <input
      type="checkbox"
      class="checkbox checkbox-sm"
      :checked="todo.isCompleted"
      @change="emit('toggle-complete')"
    />
    <span
      :class="{
        'line-through text-base-content/60': todo.isCompleted,
      }"
    >
      {{ todo.title }}
    </span>

    <button
      type="button"
      class="btn btn-ghost btn-xs text-error ml-auto"
      @click="emit('delete')"
    >
      ✕
    </button>
  </li>
</template>
