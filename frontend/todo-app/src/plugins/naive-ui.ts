import {
  create,
  NButton,
  NInput,
  NList,
  NListItem,
  NCheckbox,
  NSpin,
  NAlert,
  NCard,
  NSpace,
} from 'naive-ui';

export function createNaiveUI() {
  return create({
    components: [
      NButton,
      NInput,
      NList,
      NListItem,
      NCheckbox,
      NSpin,
      NAlert,
      NCard,
      NSpace,
    ],
  });
}
