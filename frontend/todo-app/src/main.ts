import { createApp } from 'vue'
import App from './App.vue'
import './style.css';

import {
  VueQueryPlugin,
  type VueQueryPluginOptions,
} from '@tanstack/vue-query';
import { createNaiveUI } from './plugins/naive-ui';

const app = createApp(App);

// Vue Query
const vueQueryOptions: VueQueryPluginOptions = {
  queryClientConfig: {
    defaultOptions: {
      queries: {
        retry: 1,
        refetchOnWindowFocus: false,
      },
    },
  },
};

app.use(VueQueryPlugin, vueQueryOptions);

// UI library
app.use(createNaiveUI());
app.mount('#app')
