import { fireEvent } from '@testing-library/vue'
import { renderWithQuery } from '../utils/renderWithQuery'
import SingleTodoItem from '@/components/SingleTodoItem.vue'

describe('TodoItem component events', () => {
  const todo = {
    id: 1,
    text: 'test',
    completed: false,
  }

  test('emits toggle event when checkbox is clicked', async () => {
    const { getByRole, emitted } = renderWithQuery(SingleTodoItem, {
      props: { todo },
    })

    const checkbox = getByRole('checkbox')
    await fireEvent.click(checkbox)

    expect(emitted()).toHaveProperty('toggle-complete')
  })

  test('emits delete event when delete button is clicked', async () => {
    const { getByTestId, emitted } = renderWithQuery(SingleTodoItem, {
      props: { todo },
    })

    await fireEvent.click(getByTestId('todo-delete-button'))

    expect(emitted()).toHaveProperty('delete')
  })
})