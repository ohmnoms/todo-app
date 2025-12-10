import { render } from '@testing-library/vue'
import { QueryClient, VueQueryPlugin } from '@tanstack/vue-query'
import type { RenderOptions } from '@testing-library/vue'

export function renderWithQuery(component: unknown, options: RenderOptions = {}) {
  const queryClient = new QueryClient({
    defaultOptions: { queries: { retry: false } },
  })

  return render(component, {
    ...options,
    global: {
      ...(options.global ?? {}),
      plugins: [
        ...(options.global?.plugins ?? []),
        [VueQueryPlugin, { queryClient }],
      ],
    },
  })
}
