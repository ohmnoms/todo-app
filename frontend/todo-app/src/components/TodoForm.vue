<script setup lang="ts">
import { fromDateTimeLocal, toDateTimeLocal } from '@/helpers/dateHelpers';
import { maxTitleLength, validateCompletedDate, validateDueDate, validateDueTime, validateTodoTitle } from '@/helpers/todoValidation';
import type { CreateTodoRequest } from '@/models/create-todo-request';
import type { TodoItem } from '@/models/todo-item';
import type { UpdateTodoRequest } from '@/models/update-todo-request';
import { computed, reactive, ref, watch } from 'vue';
import { useDeviceId } from '@/composables/useDeviceId';

const props = defineProps<{
  busy?: boolean;
  todo?: TodoItem | null;
}>();

const emit = defineEmits<{
  (e: 'submit', payload: CreateTodoRequest): void;
  (e: 'update', payload: UpdateTodoRequest): void;
  (e: 'cancel'): void;
}>();

const title = ref(props.todo?.title ?? '');
const dueDate = ref<string | null>(props.todo?.dueDate ?? null);
const dueTime = ref<string | null>(props.todo?.dueTime ?? null);
const completedDate = ref<string | null>(
  toDateTimeLocal(props.todo?.completedDate ?? null)
);
const deviceId = useDeviceId();

const touched = reactive({
  title: false,
  dueDate: false,
  dueTime: false,
  completedDate: false,
});

const errors = reactive({
  title: null as string | null,
  dueDate: null as string | null,
  dueTime: null as string | null,
  completedDate: null as string | null,
});

const isEditing = computed(() => !!props.todo);

const initialTitle = ref(title.value);
const initialDueDate = ref(dueDate.value);
const initialDueTime = ref(dueTime.value);
const initialCompletedDate = ref(completedDate.value);

const dueDateMin = (() => {
  const d = new Date();
  d.setHours(0, 0, 0, 0);
  return d.toISOString().slice(0, 10);
})();

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const completedMin = (() => {
  const d = new Date();
  d.setSeconds(0, 0);
  const tzOffset = d.getTimezoneOffset();
  const local = new Date(d.getTime() - tzOffset * 60_000);
  return local.toISOString().slice(0, 16);
})();

function validateTitleField(value: string) {
  errors.title = validateTodoTitle(value);
}

function validateDueDateField(value: string | null) {
  errors.dueDate = validateDueDate(value);
}

function validateDueTimeField(value: string | null) {
  errors.dueTime = validateDueTime(value);
}

function validateCompletedDateField(value: string | null) {
  errors.completedDate = validateCompletedDate(value);
}

const isFormValid = computed(
  () =>
    !errors.title &&
    !errors.dueDate &&
    !errors.dueTime &&
    !errors.completedDate &&
    !!title.value.trim(),
);

const isDirty = computed(() => {
  if (!isEditing.value) {
    return (
      !!title.value.trim() || 
      !!dueDate.value || 
      !!dueTime.value || 
      !!completedDate.value
    );
  }

  const sameTitle = (title.value ?? '') === (initialTitle.value ?? '');
  const sameDate = (dueDate.value ?? null) === (initialDueDate.value ?? null);
  const sameTime = (dueTime.value ?? null) === (initialDueTime.value ?? null);
  const sameCompletedDate = 
    (completedDate.value ?? null) === (initialCompletedDate.value ?? null);
  return !(sameTitle && sameDate && sameTime && sameCompletedDate);
});

function markAllTouched() {
  touched.title = true;
  touched.dueDate = true;
  touched.dueTime = true;
  touched.completedDate = true;
}

function resetForm() {
  touched.title = false;
  touched.dueDate = false;
  touched.dueTime = false;
  touched.completedDate = false;

  errors.title = null;
  errors.dueDate = null;
  errors.dueTime = null;
  errors.completedDate = null;

  if (props.todo) {
    title.value = props.todo.title;
    dueDate.value = props.todo.dueDate ?? null;
    dueTime.value = props.todo.dueTime ?? null;
    completedDate.value = props.todo.completedDate ?? null;
  } else {
    title.value = '';
    dueDate.value = null;
    dueTime.value = null;
    completedDate.value = null;
  }

  initialTitle.value = title.value;
  initialDueDate.value = dueDate.value;
  initialDueTime.value = dueTime.value;
  initialCompletedDate.value = completedDate.value;
}

function onSubmit() {
  markAllTouched();
  validateTitleField(title.value);
  validateDueDateField(dueDate.value);
  validateDueTimeField(dueTime.value);
  validateCompletedDateField(completedDate.value);

  if (!isFormValid.value) return;

  if (isEditing.value) {
    // Update Path
    const payload: UpdateTodoRequest = {
      id: props.todo!.id,
      title: title.value.trim(),
      dueDate: dueDate.value || null,
      dueTime: dueTime.value || null,
      isCompleted: props.todo!.isCompleted,
      completedDate: fromDateTimeLocal(completedDate.value),
      createdBy: deviceId,
    };
    emit('update', payload);
    return;
  } 

  // Create Path
  const payload: CreateTodoRequest = {
    title: title.value.trim(),
    dueDate: dueDate.value || null,
    dueTime: dueTime.value || null,
    completedDate: fromDateTimeLocal(completedDate.value),
    createdBy: deviceId,
  };
  emit('submit', payload);
  resetForm();
}

function onCancel() {
  resetForm();
  emit('cancel');
}

function onTitleBlur() {
  touched.title = true;
  validateTitleField(title.value);
}

function onDueDateBlur() {
  touched.dueDate = true;
  validateDueDateField(dueDate.value);

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

// eslint-disable-next-line @typescript-eslint/no-unused-vars
function onCompletedDateBlur() {
  touched.completedDate = true;
  validateCompletedDateField(completedDate.value);
}

// helper to clear time
function onClearTime() {
  dueTime.value = null;
  errors.dueTime = null;
  touched.dueTime = false;
}

watch(
  () => props.todo,
  (newTodo) => {
    if (newTodo) {
      title.value = newTodo.title;
      dueDate.value = newTodo.dueDate ?? null;
      dueTime.value = newTodo.dueTime ?? null;
      completedDate.value = toDateTimeLocal(newTodo.completedDate ?? null);
    } else {
      title.value = '';
      dueDate.value = null;
      dueTime.value = null;
      completedDate.value = null;
    }

    resetForm();
  },
  { immediate: true },
);

// Watchers for validation on change after touched
watch(title, (v) => {
  if (touched.title) validateTitleField(v);
});
watch(dueDate, (v) => {
  if (touched.dueDate) validateDueDateField(v);
});
watch(dueTime, (v) => {
  if (touched.dueTime) validateDueTimeField(v);
});
watch(completedDate, (v) => {
  if (touched.completedDate) validateCompletedDateField(v);
});

</script>

<template>
  <form
    data-testid="todo-form"
    class="w-full overflow-x-hidden"
    @submit.prevent="onSubmit()"
  >
    <div
      class="flex flex-wrap md:flex-nowrap items-start gap-2 w-full"
    >
      <div class="flex-1 min-w-0">
        <label class="text-xs text-base-content/70"
          :class="{
            'text-error': touched.title && errors.title,
          }"
        >
          {{ title.length }} / {{ maxTitleLength }}
        </label>
        <input
          v-model="title"
          data-testid="todo-input"
          type="text"
          placeholder="What to do, what to do..."
          :maxlength="maxTitleLength"
          @blur="onTitleBlur"
          class="w-full bg-transparent border-none border-b border-base-300/60 focus:border-primary focus:outline-none focus:ring-0 text-base placeholder:text-base-content/60 whitespace-normal break-words"
          
        />
      </div>

      <div
        class="flex flex-wrap md:flex-nowrap items-center justify-end gap-2 md:gap-3 flex-shrink-0 w-full md:w-auto"
      >
        <div class="flex flex-col items-start">
          <label 
            class="text-[0.65rem] uppercase tracking-wide text-base-content/60"
            :class="{
              'text-error': touched.dueDate && errors.dueDate,
            }"
          >
            Due date
          </label>
          <input
            v-model="dueDate"
            :min="dueDateMin"
            type="date"
            @blur="onDueDateBlur"
            class="bg-transparent px-3 py-1 rounded-full border border-transparent hover:border-base-300/80 focus:border-primary focus:outline-none cursor-pointer text-xs"
          />
          <p
            v-if="touched.dueDate && errors.dueDate"
            class="mt-1 text-xs text-error"
          >
            {{ errors.dueDate }}
          </p>
        </div>

        <div
          v-if="dueDate"
          class="flex flex-col items-start"
        >
          <label class="text-[0.65rem] uppercase tracking-wide text-base-content/60">
            Time
          </label>
          <div class="flex items-center gap-1">
            <input
              v-model="dueTime"
              type="time"
              @blur="onDueTimeBlur"
              class="bg-transparent px-3 py-1 rounded-full border border-transparent hover:border-base-300/80 focus:border-primary focus:outline-none cursor-pointer text-xs"
            />
            <button
              v-if="dueTime"
              type="button"
              class="btn btn-ghost btn-xs btn-circle"
              title="Clear time"
              @click="onClearTime"
            >
              <span class="material-icons text-base-content/70">
                close
              </span>
            </button>
          </div>
        </div>
        
        <!--TODO: Ran out of time trying to make completed picker work-->
        <!-- <div
          v-if="isEditing && props.todo?.isCompleted"
          class="flex flex-col items-start"
        >
          <label class="text-[0.65rem] uppercase tracking-wide text-base-content/60">
            Completed
          </label>
          <input
            v-model="completedDate"
            type="datetime-local"
            :min="completedMin"
            @blur="onCompletedDateBlur"
            class="bg-transparent px-3 py-1 rounded-full border border-transparent hover:border-base-300/80 focus:border-primary focus:outline-none cursor-pointer text-xs"
            :class="{ 'border-error': touched.completedDate && errors.completedDate }"
          />
          <p
            v-if="touched.completedDate && errors.completedDate"
            class="mt-1 text-xs text-error"
          >
            {{ errors.completedDate }}
          </p>
        </div> -->

        <div class="flex items-center gap-1 ml-auto md:ml-0">
          <button
            v-if="!isEditing"
            type="submit"
            class="btn btn-primary btn-sm btn-circle"
            :disabled="props.busy || !isFormValid"
            title="Add task"
          >
            <span class="material-icons">
              add_task
            </span>
          </button>

          <template v-else>
            <button
              type="submit"
              class="btn btn-primary btn-sm btn-circle"
              :disabled="props.busy || !isFormValid || !isDirty"
              :title="isDirty ? 'Save changes' : 'No changes to save'"
            >
              <span class="material-icons">
                check
              </span>
            </button>
            <button
              type="button"
              class="btn btn-ghost btn-sm btn-circle"
              title="Cancel edit"
              @click="onCancel"
            >
              <span class="material-icons">
                close
              </span>
            </button>
          </template>
        </div>
      </div>
    </div>
  </form>
</template>
