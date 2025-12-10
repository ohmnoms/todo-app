<script setup lang="ts">
import type { TodoItem } from '@/models/todo-item';
import { ref } from 'vue';
import TodoForm from './TodoForm.vue';
import type { UpdateTodoRequest } from '@/models/update-todo-request';
import { toDateTimeLocal } from '@/helpers/dateHelpers';

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const props = defineProps<{
  todo: TodoItem;
}>();

const emit = defineEmits<{
  (e: 'toggle-complete'): void;
  (e: 'delete'): void;
  (e: 'update', payload: UpdateTodoRequest): void;
}>();

const isEditing = ref(false);

function startEdit() {
  isEditing.value = true;
}

function cancelEdit() {
  isEditing.value = false;
}

function submitEdit(payload: UpdateTodoRequest) {
  emit('update', payload);
  isEditing.value = false;
}

function isOverdue(todo: TodoItem): boolean {
  if (!todo.dueDate || todo.isCompleted) {
    return false;
  }
  
  const [year, month, day] = todo.dueDate.split('-').map(Number);
  if (!year || !month || !day) {
    return false; // bad date
  }

  let due: Date;
  
  if (todo.dueTime) {
    // compare exact local date+time
    const [hours, minutes] = todo.dueTime.split(':').map(Number);
    due = new Date(year, month - 1, day, hours ?? 0, minutes ?? 0, 0, 0);
  } else {
    // consider due at EOD
    due = new Date(year, month - 1, day, 23, 59, 59, 999);
  }

  return due.getTime() < Date.now();
}
</script>

<template>
  <li class="w-full overflow-x-hidden" data-testid="todo-item">
    <TodoForm
      v-if="isEditing"
      :todo="todo"
      @update="submitEdit"
      @cancel="cancelEdit"
    />

    <div
      v-else
      class="flex flex-wrap md:flex-nowrap items-start gap-2 w-full"
    >
      <div class="pt-1 flex-shrink-0">
        <input
          type="checkbox"
          data-testid="todo-toggle"
          class="checkbox checkbox-sm"
          :checked="todo.isCompleted"
          @change="emit('toggle-complete')"
        />
      </div>

      <div
        class="flex-1 min-w-0 whitespace-normal break-words cursor-text"
        @click="startEdit"
      >
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
          class="ml-2 text-xs text-base-content/60"
          :class="{
            'text-error': isOverdue(todo),
          }"
        >
          (due {{ todo.dueDate }}<span v-if="todo.dueTime"> @ {{ todo.dueTime }}</span>)
        </span>
        
        <span
          v-if="todo.isCompleted && todo.completedDate"
          class="ml-2 text-xs text-base-content/60"
        >
          (completed {{ toDateTimeLocal(todo.completedDate)?.replace('T', ' @ ') }})
        </span>
      </div>

      <div class="ml-auto flex items-center gap-1 flex-shrink-0">
        <button
          type="button"
          class="btn btn-ghost btn-xs btn-circle text-accent"
          data-testid="todo-edit-button"
          @click.stop="startEdit"
          title="Edit todo"
        >
          <span class="material-icons">
            edit
          </span>
        </button>

        <button
          type="button"
          class="btn btn-ghost btn-xs btn-circle text-error"
          data-testid="todo-delete-button"
          @click="emit('delete')"
          title="Delete todo"
        >
          <span class="material-icons">
            delete
          </span>
        </button>
      </div>
    </div>
  </li>
</template>
