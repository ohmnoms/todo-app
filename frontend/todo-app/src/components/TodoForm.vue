<script setup lang="ts">
import { maxTitleLength, validateDueDate, validateDueTime, validateTodoTitle } from '@/helpers/todoValidation';
import type { TodoItem } from '@/models/todo-item';
import { computed, reactive, ref, watch } from 'vue';

const props = defineProps<{
  busy?: boolean;
  todo?: TodoItem | null;
}>();

const emit = defineEmits<{
  (e: 'submit', payload: { title: string; dueDate?: string | null; dueTime?: string | null }): void;
  (e: 'cancel'): void;
}>();

const title = ref(props.todo?.title ?? '');
const dueDate = ref<string | null>(props.todo?.dueDate ?? null);
const dueTime = ref<string | null>(props.todo?.dueTime ?? null);

const touched = reactive({
  title: false,
  dueDate: false,
  dueTime: false,
});

const errors = reactive({
  title: '' as string | null,
  dueDate: null as string | null,
  dueTime: null as string | null,
});

const isEditing = computed(() => !!props.todo);

function validateTitle(value: string) {
  errors.title = validateTodoTitle(value);
}
function validateDueDateField(value: string | null) {
  errors.dueDate = validateDueDate(value);
}
function validateDueTimeField(value: string | null) {
  errors.dueTime = validateDueTime(value);
}

const isFormValid = computed(() => {
  return !errors.title && !errors.dueDate && !errors.dueTime && !!title.value.trim();
});

function onSubmit() {
  // Mark all fields as touched so errors show if present
  touched.title = true;
  touched.dueDate = true;
  touched.dueTime = true;

  // Validate current values
  validateTitle(title.value);
  validateDueDateField(dueDate.value);
  validateDueTimeField(dueTime.value);
  if (!isFormValid.value) return;

  const payload = {
    title: title.value.trim(),
    dueDate: dueDate.value || null,
    dueTime: dueTime.value || null,
  };
  emit('submit', payload);
  
  if (!isEditing.value) {
    resetForm();
  }
}

function resetForm() {
  touched.title = false;
  touched.dueDate = false;
  touched.dueTime = false;
  errors.title = null;
  errors.dueDate = null;
  errors.dueTime = null;
  title.value = props.todo?.title ?? '';
  dueDate.value = props.todo?.dueDate ?? null;
  dueTime.value = props.todo?.dueTime ?? null;
}

function onTitleBlur() {
  touched.title = true;
  validateTitle(title.value);
}

function onDueDateBlur() {
  touched.dueDate = true;
  validateDueDateField(dueDate.value);
  // Clearing the date should also clear the time to keep the fields in
  // sync and avoid sending an orphaned time to the API.
  if (!dueDate.value) {
    dueTime.value = null;
    errors.dueTime = null;
    touched.dueTime = false;
  }
}

function onDueTimeBlur() {
  touched.dueTime = true;
  validateDueTimeField(dueTime.value);
}

watch(
  () => props.todo,
  (newTodo) => {
    if (newTodo) {
      title.value = newTodo.title;
      dueDate.value = newTodo.dueDate ?? null;
      dueTime.value = newTodo.dueTime ?? null;
    } else {
      title.value = '';
      dueDate.value = null;
      dueTime.value = null;
    }

    // Reset touched and errors so the new values are validated afresh
    touched.title = false;
    touched.dueDate = false;
    touched.dueTime = false;
    errors.title = null;
    errors.dueDate = null;
    errors.dueTime = null;
  }
);

// Revalidate on input changes when the corresponding field has been
// interacted with.
watch(title, (newVal) => {
  if (touched.title) {
    validateTitle(newVal);
  }
});
watch(dueDate, (newVal) => {
  if (touched.dueDate) {
    validateDueDateField(newVal);
  }
});
watch(dueTime, (newVal) => {
  if (touched.dueTime) {
    validateDueTimeField(newVal);
  }
});

function onCancel() {
  resetForm();
  emit('cancel');
}
</script>

<template>
  <form
    data-testid="todo-form"
    class="flex flex-col md:flex-row md:items-end gap-2"
    @submit.prevent="onSubmit"
  >
    <!-- Title field -->
    <div class="form-control md:flex-1">
      <label class="label">
        <span class="label-text">Title</span>
        <span class="label-text-alt text-xs text-gray-400">
          {{ title.length }} / {{ maxTitleLength }}
        </span>
      </label>
      <input
        v-model="title"
        data-testid="todo-input"
        @blur="onTitleBlur"
        :maxlength="maxTitleLength"
        type="text"
        placeholder="What do you need to do?"
        class="input input-bordered w-full"
        :class="{
          'input-error': touched.title && errors.title,
        }"
      />
      <p
        v-if="touched.title && errors.title"
        class="mt-1 text-sm text-error"
      >
        {{ errors.title }}
      </p>
    </div>

    <!-- Due date field -->
    <div class="form-control">
      <label class="label">
        <span class="label-text">Due Date</span>
      </label>
      <input
        v-model="dueDate"
        @blur="onDueDateBlur"
        type="date"
        class="input input-bordered"
        :class="{
          'input-error': touched.dueDate && errors.dueDate,
        }"
      />
      <p
        v-if="touched.dueDate && errors.dueDate"
        class="mt-1 text-sm text-error"
      >
        {{ errors.dueDate }}
      </p>
    </div>

    <!-- Due time field -->
    <div v-if="dueDate" class="form-control">
      <label class="label">
        <span class="label-text">Due Time</span>
      </label>
      <input
        v-model="dueTime"
        @blur="onDueTimeBlur"
        type="time"
        class="input input-bordered"
        :class="{
          'input-error': touched.dueTime && errors.dueTime,
        }"
      />
      <p
        v-if="touched.dueTime && errors.dueTime"
        class="mt-1 text-sm text-error"
      >
        {{ errors.dueTime }}
      </p>
    </div>

    <!-- Buttons -->
    <div class="flex gap-2 md:ml-auto">
      <button
        class="btn btn-primary"
        type="submit"
        :disabled="props.busy || !isFormValid"
      >
        {{ isEditing ? 'Save' : 'Add' }}
      </button>
      <button
        v-if="isEditing"
        class="btn btn-ghost"
        type="button"
        @click="onCancel"
      >
        Cancel
      </button>
    </div>
  </form>
</template>
