import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import TodoPage from '@/pages/TodoPage.vue';
import SettingsPage from '@/pages/SettingsPage.vue';

const routes: RouteRecordRaw[] = [
  { path: '/', redirect: '/home' },
  { path: '/home', name: 'Home', component: TodoPage },
  { path: '/settings', name: 'Settings', component: SettingsPage }
];

export default createRouter({
  history: createWebHistory(),
  routes
});
