<script setup lang="ts">
import ToasterMessage from './components/ToasterMessage.vue';
import TodoPage from './views/TodoPage.vue';
</script>

<template>
  <div class="app-shell">
    <TodoPage />
  </div>
  <ToasterMessage />
</template>
