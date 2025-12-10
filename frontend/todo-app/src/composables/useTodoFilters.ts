import { ref, watch } from 'vue';

const HIDE_COMPLETED_KEY = 'hideCompleted';

const hideCompleted = ref<boolean>(false);

// initialize from localStorage once
if (typeof window !== 'undefined') {
  const stored = localStorage.getItem(HIDE_COMPLETED_KEY);
  hideCompleted.value = stored === 'true';

  watch(hideCompleted, (value) => {
    localStorage.setItem(HIDE_COMPLETED_KEY, String(value));
  });
}

export function useTodoFilters() {
  return { hideCompleted };
}
