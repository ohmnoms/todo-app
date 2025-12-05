<script setup lang="ts">
import { ref } from 'vue';

defineProps<{
  busy?: boolean;
}>();

const emit = defineEmits<{
  (e: 'submit', title: string): void;
}>();

const title = ref('');

function onSubmit() {
  const trimmed = title.value.trim();
  if (!trimmed) return;

  emit('submit', trimmed);
  title.value = '';
}
</script>

<template>
  <form class="flex gap-2" @submit.prevent="onSubmit">
    <input
      v-model="title"
      type="text"
      placeholder="What do you need to do?"
      class="input input-bordered flex-1"
    />
    <button
      class="btn btn-primary"
      type="submit"
      :disabled="busy"
    >
      Add
    </button>
  </form>
</template>
