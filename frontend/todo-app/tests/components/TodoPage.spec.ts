import { fireEvent, screen } from '@testing-library/vue'
import { vi } from 'vitest'
import { renderWithQuery } from '../utils/renderWithQuery'

import TodoPage from '@/views/TodoPage.vue'

// mock composable
vi.mock('@/composables/useTodos', () => {
  const todos = [
    { id: '1', title: 'Existing todo', isCompleted: false },
  ]

  const createTodo = { isPending: { value: false }, mutate: vi.fn() }
  const updateTodo = { isPending: { value: false }, mutate: vi.fn() }
  const deleteTodo = { isPending: { value: false }, mutate: vi.fn() }

  return {
    useTodos: () => ({
      todos,
      todosQuery: { isPending: { value: false } },
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
    renderWithQuery(TodoPage)

    const input = screen.getByTestId('todo-input')
    await fireEvent.update(input, 'Do the thing')

    const form = screen.getByTestId('todo-form')
    await fireEvent.submit(form)

    expect(createTodo.mutate).toHaveBeenCalledWith('Do the thing')
  })

  test('calls deleteTodo.mutate when delete button clicked', async () => {
    renderWithQuery(TodoPage)

    const deleteBtn = screen.getByTestId('todo-delete-button')
    await fireEvent.click(deleteBtn)

    expect(deleteTodo.mutate).toHaveBeenCalledWith('1')
  })

  test('calls updateTodo.mutate when toggle clicked', async () => {
    renderWithQuery(TodoPage)

    const toggle = screen.getByTestId('todo-toggle')
    await fireEvent.click(toggle)

    expect(updateTodo.mutate).toHaveBeenCalledWith({
      id: '1',
      patch: {
        id: '1',
        title: 'Existing todo',
        isCompleted: true, // toggled
      },
    })
  })
})