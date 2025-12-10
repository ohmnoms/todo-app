import { ref, watch } from 'vue';

const THEME_KEY = 'theme';
type Theme = 'light' | 'dark';

function getPreferredTheme(): Theme {
  const stored = localStorage.getItem(THEME_KEY) as Theme | null;
  if (stored === 'light' || stored === 'dark') return stored;

  // fall back to system preference
  return window.matchMedia('(prefers-color-scheme: dark)').matches
    ? 'dark'
    : 'light';
}

export function applyTheme(theme: Theme) {
  // DaisyUI looks at data-theme on <html> or <body>
  document.documentElement.setAttribute('data-theme', theme);
}

export function useTheme() {
  const theme = ref<Theme>(getPreferredTheme());

  // apply immediately and on change
  applyTheme(theme.value);

  watch(theme, (value) => {
    applyTheme(value);
    localStorage.setItem(THEME_KEY, value);
  });

  return { theme };
}
