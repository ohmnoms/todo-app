<script setup lang="ts">
import { validateTodoTitle } from '@/helpers/todoValidation';
import { computed, reactive, ref, watch } from 'vue';

defineProps<{
  busy?: boolean;
}>();

const emit = defineEmits<{
  (e: 'submit', title: string): void;
}>();

const title = ref('');

const touched = reactive({
  title: false,
});

const errors = reactive({
  title: '' as string | null,
});

const maxTitleLength = 240;

function validateTitle(value: string) {
  errors.title = validateTodoTitle(value);
}

const isFormValid = computed(() => {
  return !errors.title && !!title.value.trim();
});

function onSubmit() {
  const trimmed = title.value.trim();
  if (!trimmed) return;

  emit('submit', trimmed);
  resetForm();
}

function resetForm() {
  touched.title = false;
  errors.title = null;
  title.value = '';
}

function onTitleBlur() {
  touched.title = true;
  validateTitle(title.value);
}

watch(title, (newVal) => {
  if (touched.title) {
    validateTitle(newVal);
  }
});
</script>

<template>
  <form data-testid="todo-form" class="flex gap-2" @submit.prevent="onSubmit">
    <div class="form-control">
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
    <button
      class="btn btn-primary"
      type="submit"
      :disabled="busy || !isFormValid"
    >
      Add
    </button>
  </form>
</template>
