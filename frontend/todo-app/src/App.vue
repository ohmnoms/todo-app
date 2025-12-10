<script setup lang="ts">
import ToasterMessage from '@/components/ToasterMessage.vue';
import { useTheme } from './composables/useTheme';

useTheme();
</script>

<template>
  <div class="app-shell">
    <RouterView />
  </div>
  <ToasterMessage />
</template>
