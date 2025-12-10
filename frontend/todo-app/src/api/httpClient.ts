const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:3000';

async function request<T>(input: string, init?: RequestInit): Promise<T> {
	const res = await fetch(`${BASE_URL}${input}`, {
		headers: {
			'Content-Type': 'application/json',
			...(init?.headers ?? {}),
		},
		...init,
	});

	if (!res.ok) {
		const errorBody = await res.text().catch(() => '');
		throw new Error(`HTTP ${res.status}: ${errorBody || res.statusText}`);
	}
	
	return (await res.json()) as T;
}

async function requestVoid(input: string, init?: RequestInit): Promise<void> {
  const res = await fetch(`${BASE_URL}${input}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(init?.headers ?? {}),
    },
    ...init,
  });

  if (!res.ok) {
    const errorBody = await res.text().catch(() => '');
    throw new Error(`HTTP ${res.status}: ${errorBody || res.statusText}`);
  }

  return;
}

function buildQuery(params?: Record<string, unknown>): string {
  if (!params) return '';
  const qs = new URLSearchParams();
  Object.entries(params).forEach(([key, value]) => {
    if (value === null || value === undefined) return;
    qs.set(key, String(value));
  });
  const s = qs.toString();
  return s ? `?${s}` : '';
}

export const httpClient = {
	get<T>(url: string, query?: Record<string, unknown>, init?: RequestInit) {
		const fullUrl = `${url}${buildQuery(query)}`;
		return request<T>(fullUrl, {
		method: 'GET',
		...init,
		});
	},
	post<T>(url: string, body?: unknown, init?: RequestInit) {
		return request<T>(url, {
			method: 'POST',
			body: body ? JSON.stringify(body) : undefined,
			...init,
		});
	},
	put<T>(url: string, body?: unknown, init?: RequestInit) {
		return request<T>(url, {
			method: 'PUT',
			body: body ? JSON.stringify(body) : undefined,
			...init,
		});
	},
	patch<T>(url: string, body?: unknown, init?: RequestInit) {
		return request<T>(url, {
			method: 'PATCH',
			body: body ? JSON.stringify(body) : undefined,
			...init,
		});
	},
	delete(url: string, init?: RequestInit) {
		return requestVoid(url, { method: 'DELETE', ...init });
	},
};
