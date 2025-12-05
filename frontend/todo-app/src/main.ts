import { createApp } from 'vue'
import App from './App.vue'
import './main.css';

import {
  VueQueryPlugin,
  type VueQueryPluginOptions,
} from '@tanstack/vue-query';
import { showErrorToast } from './composables/useToasts';

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

// Global error handler for unhandled exceptions
app.config.errorHandler = (err, instance, info) => {
  console.error('Vue global error:', err, info);
  showErrorToast('An unexpected error occurred. Please try again.');
};


app.mount('#app')
