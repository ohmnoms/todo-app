import { fireEvent, screen } from '@testing-library/vue'
import { vi } from 'vitest'
import { renderWithQuery } from '../utils/renderWithQuery'
import("@/models/todo-item")

import TodoPage from '@/pages/TodoPage.vue'

// mock composable
vi.mock('@/composables/useTodos', () => {
  const todos = [
    { 
      id: '1', 
      title: 'Existing todo', 
      isCompleted: false, 
      createdBy: 'device-id' 
    },
  ]

  const createTodo = { isPending: { value: false }, mutate: vi.fn() }
  const updateTodo = { isPending: { value: false }, mutate: vi.fn() }
  const deleteTodo = { isPending: { value: false }, mutate: vi.fn() }

  const todosQuery = vi.fn().mockReturnValue({
    isPending: { value: false },
    data: { value: todos },
  });

  return {
    useTodos: () => ({
      todos,
      todosQuery,
      createTodo,
      updateTodo,
      deleteTodo,
    }),
  }
})

const mockedUseTodos = await vi.importMock<unknown>('@/composables/useTodos')
const { useTodos } = mockedUseTodos
const { createTodo, updateTodo, deleteTodo } = useTodos()

describe('TodoPage', () => {
  test('calls createTodo.mutate when form submitted', async () => {
    // Arrange
    const expectation = 
    {
      "completedDate": null,
      "dueDate": null,
      "dueTime": null,
      "title": "Do the thing",
    } as Partial<TodoItem>;

    // Act
    renderWithQuery(TodoPage)

    const input = screen.getByTestId('todo-input')
    await fireEvent.update(input, 'Do the thing')

    const form = screen.getByTestId('todo-form')
    await fireEvent.submit(form)

    // Assert
    expect(createTodo.mutate).toHaveBeenCalledWith(expectation)
  })

  test('calls deleteTodo.mutate when delete button clicked', async () => {
    renderWithQuery(TodoPage)

    const deleteBtn = screen.getByTestId('todo-delete-button')
    await fireEvent.click(deleteBtn)

    expect(deleteTodo.mutate).toHaveBeenCalledWith('1')
  })

  test('calls updateTodo.mutate when toggle clicked', async () => {
    renderWithQuery(TodoPage);
    
    const beforeToggle = Date.now(); // capture just before action
    const toggle = screen.getByTestId('todo-toggle');
    await fireEvent.click(toggle);

    // Assert mutate was called correctly
    expect(updateTodo.mutate).toHaveBeenCalled();

    const call = updateTodo.mutate.mock.calls[0][0];

    expect(call).toMatchObject({
      id: '1',
      patch: {
        id: '1',
        title: 'Existing todo',
        isCompleted: true,
        createdBy: 'device-id'
        // completedDate checked below
      }
    });

    // Validate completedDate is within 1 second of beforeToggle
    const completedDate = new Date(call.patch.completedDate).getTime();

    expect(Math.abs(completedDate - beforeToggle)).toBeLessThanOrEqual(1000);
  });
})