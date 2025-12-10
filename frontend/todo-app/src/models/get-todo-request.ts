export interface GetTodoRequest {
  createdBy?: string;
  showCompleted?: boolean;
  // satisfies Record<string, unknown> for httpClient compatibility
  [key: string]: unknown;
}