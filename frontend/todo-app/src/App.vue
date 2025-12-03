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
  <div class="app-shell">
    <div class="card bg-base-100 shadow-lg">
      <div class="card-body space-y-4">
        <h2 class="card-title">Do It</h2>

        <div class="flex gap-2">
          <input
            v-model="title"
            type="text"
            placeholder="What do you need to do?"
            class="input input-bordered flex-1"
          />
          <button class="btn btn-primary" @click="addTodo">
            Add
          </button>
        </div>

        <div v-if="todos.length === 0" class="alert alert-info">
          <span>No todos yet. Add your first one above.</span>
        </div>

        <ul v-else class="space-y-2">
          <li
            v-for="todo in todos"
            :key="todo"
            class="flex items-center gap-2"
          >
            <input type="checkbox" class="checkbox checkbox-sm" />
            <span>{{ todo }}</span>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>
