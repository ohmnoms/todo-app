<script setup lang="ts">
import { useToasts } from '@/composables/useToasts';

const { toasts, dismissToast } = useToasts();
</script>

<template>
  <div class="fixed bottom-4 right-4 space-y-2 z-50">
    <div
      v-for="toast in toasts"
      :key="toast.id"
      class="px-4 py-2 rounded shadow-lg cursor-pointer"
      :class="{
        'bg-red-500 text-white': toast.type === 'error',
        'bg-green-500 text-white': toast.type === 'success',
        'bg-slate-800 text-white': toast.type === 'info',
      }"
      @click="dismissToast(toast.id)"
    >
      {{ toast.message }}
    </div>
  </div>
</template>
