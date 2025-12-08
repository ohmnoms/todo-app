
<script setup lang="ts">
import type { TodoItem } from '@/models/todo-item';
import { ref } from 'vue';

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const props = defineProps<{
  todo: TodoItem;
}>();

const emit = defineEmits<{
  (e: 'toggle-complete'): void;
  (e: 'delete'): void;
  (e: 'update', payload: { title: string; dueDate?: string | null; dueTime?: string | null }): void;
}>();

const isEditing = ref(false);

function startEdit() {
  isEditing.value = true;
}
function cancelEdit() {
  isEditing.value = false;
}
function submitEdit(payload: { title: string; dueDate?: string | null; dueTime?: string | null }) {
  emit('update', payload);
  isEditing.value = false;
}
</script>

<template>
  <li class="flex items-center gap-2" data-testid="todo-item">
    <TodoForm
      v-if="isEditing"
      :todo="todo"
      @submit="submitEdit"
      @cancel="cancelEdit"
    />
    <div v-else class="flex items-center gap-2">
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

      <span
        v-if="todo.dueDate"
        class="text-xs text-gray-500"
      >
        (due {{ todo.dueDate }}<span v-if="todo.dueTime"> {{ todo.dueTime }}</span>)
      </span>
      <!-- Edit and delete buttons align to the right -->
      <div class="ml-auto flex gap-1">
        <button
          type="button"
          class="btn btn-ghost btn-xs"
          data-testid="todo-edit-button"
          @click="startEdit"
        >
          ✎
        </button>
        <button
          type="button"
          class="btn btn-ghost btn-xs text-error"
          data-testid="todo-delete-button"
          @click="emit('delete')"
        >
          ✕
        </button>
      </div>
    </div>
  </li>
</template>
