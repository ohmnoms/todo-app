import { createApp } from 'vue'
import App from './App.vue'
import './main.css';

import {
  VueQueryPlugin,
  type VueQueryPluginOptions,
} from '@tanstack/vue-query';

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

app.mount('#app')
