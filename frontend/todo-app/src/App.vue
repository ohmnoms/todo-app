<script setup lang="ts">
  import { ref } from 'vue';
  const title = ref('');
  const todos = ref<string[]>([]);

  function addTodo() {
    if (!title.value.trim()) return;
    todos.value.push(title.value.trim());
    title.value = '';
  }
</script>

<template>
  <div style="max-width: 600px; margin: 2rem auto;">
    <n-card title="Todo MVP (Vue)">
      <n-space vertical size="large">
        <!-- Input + button row -->
        <n-space>
          <n-input
            v-model:value="title"
            placeholder="What do you need to do?"
          />
          <n-button type="primary" @click="addTodo">
            Add
          </n-button>
        </n-space>

        <!-- List -->
        <div v-if="todos.length === 0">
          <n-alert type="info" title="No todos yet">
            Add your first todo above.
          </n-alert>
        </div>
        <n-list v-else>
          <n-list-item v-for="todo in todos" :key="todo">
            {{ todo }}
          </n-list-item>
        </n-list>
      </n-space>
    </n-card>
  </div>
</template>

<style scoped></style>
