import { readonly, ref } from 'vue';

export type ToastType = 'info' | 'success' | 'error';

export interface Toast {
  id: number;
  message: string;
  type: ToastType;
}

const toasts = ref<Toast[]>([]);
let nextId = 1;

function showToast(message: string, type: ToastType = 'info', timeout = 4000) {
  const toast: Toast = { id: nextId++, message, type };
  toasts.value = [...toasts.value, toast];

  if (timeout > 0) {
    setTimeout(() => dismissToast(toast.id), timeout);
  }
}

function dismissToast(id: number) {
  toasts.value = toasts.value.filter(t => t.id !== id);
}

export function useToasts() {
  return {
    toasts: readonly(toasts),
    showToast,
    dismissToast,
  };
}

export function showErrorToast(message: string) {
  showToast(message, 'error');
}
