import { showErrorToast } from '@/composables/useToasts';

function normalizeError(err: unknown): string {
  if (err instanceof Error) return err.message;
  if (typeof err === 'string') return err;
  try {
    return JSON.stringify(err);
  } catch {
    return 'Unknown error';
  }
}

export function useErrorHandler() {
  function handleError(err: unknown, context?: string) {
    const core = normalizeError(err);
    const msg = context ? `${context}: ${core}` : core;

    console.error('[ErrorHandler]', err);
    showErrorToast(msg);
  }

  return { handleError };
}
