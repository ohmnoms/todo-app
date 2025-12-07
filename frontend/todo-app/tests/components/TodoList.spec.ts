import { render, fireEvent, screen } from '@testing-library/vue'
import TodoList from '@/components/TodoList.vue'


describe('TodoList', () => {
    const todos = [
        { id: '1', title: 'One', isCompleted: false },
        { id: '2', title: 'Two', isCompleted: true },
    ]

  test('renders a list of todos', () => {
    render(TodoList, { props: { todos } })

    const items = screen.getAllByTestId('todo-item')
    expect(items).toHaveLength(2)
  })

  test('bubbles toggle-complete with id', async () => {
    const { emitted } = render(TodoList, { props: { todos } })

    const toggles = screen.getAllByTestId('todo-toggle')
    await fireEvent.click(toggles[0])

    expect(emitted()['toggle-complete'][0]).toEqual(['1'])
  })

  test('bubbles delete with id', async () => {
    const { emitted } = render(TodoList, { props: { todos } })

    const deletes = screen.getAllByTestId('todo-delete-button')
    await fireEvent.click(deletes[1])

    expect(emitted().delete[0]).toEqual(['2'])
  })
})
