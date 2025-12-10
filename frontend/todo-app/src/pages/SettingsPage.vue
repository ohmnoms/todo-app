<script setup lang="ts">
import { ref, onMounted } from 'vue';
import ConfirmResetDeviceIdModal from '../components/ConfirmResetDeviceIdModal.vue';
import { useDeviceId, resetDeviceId } from '../composables/useDeviceId';
import { applyTheme } from '@/composables/useTheme';

const DEVICE_THEME_KEY = 'theme';
const deviceId = ref('');
const openModal = ref(false);

type Theme = 'light' | 'dark';

const theme = ref<Theme>('light');

function loadSettings() {
  const stored = localStorage.getItem(DEVICE_THEME_KEY) as Theme | null;
  theme.value =
    stored ??
    (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');

  deviceId.value = useDeviceId();
}

function onThemeChange(event: Event) {
  const input = event.target as HTMLInputElement;
  const value = input.checked ? 'dark' : 'light';
  theme.value = value as Theme;
  localStorage.setItem('theme', value);
  applyTheme(value);
}

function resetId() {
  const newId = resetDeviceId();
  deviceId.value = newId;
  openModal.value = false;
  // reload so new id is used throughout
  window.location.reload();
}

onMounted(() => {
  loadSettings();
});
</script>

<template>
  <div class="container mx-auto p-4 space-y-6">
    <header class="flex items-center gap-2 mb-4">
      <router-link to="/home" class="btn btn-ghost btn-circle">
        <span class="material-icons">home</span>
      </router-link>
      <h2 class="text-2xl font-bold">Settings</h2>
    </header>
    <div class="flex items-center justify-between">
      <span>Dark mode</span>
      <input
        type="checkbox"
        class="toggle theme-controller"
        value="dark"
        :checked="theme === 'dark'"
        @change="onThemeChange"
      />
    </div>
    <div class="border rounded-lg p-4">
      <h3 class="font-bold mb-2">Device ID</h3>
      <p class="break-all mb-2"><code>{{ deviceId }}</code></p>
      <button class="btn btn-warning" @click="openModal = true">Reset Device ID</button>
    </div>
    <ConfirmResetDeviceIdModal
      :open="openModal"
      @cancel="openModal = false"
      @confirm="resetId"
    />
  </div>
</template>