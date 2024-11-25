import { createApp } from 'vue';
import App from './App.vue';
import axios from 'axios';
import VueAxios from 'vue-axios';
import ElementPlus from 'element-plus';
import 'element-plus/dist/index.css';

// Set a default base URL for all Axios requests
axios.defaults.baseURL = 'https://localhost:7230/api';

// Add a request interceptor
// axios.interceptors.request.use(
//   (config) => {
//     // Add custom headers (e.g., authorization token)
//     const token = localStorage.getItem('authToken'); // Example: Retrieve token from localStorage
//     if (token) {
//       config.headers['Authorization'] = `Bearer ${token}`;
//     }
//     console.log('Request Intercepted:', config);
//     return config;
//   },
//   (error) => {
//     // Handle the error
//     console.error('Request Error:', error);
//     return Promise.reject(error);
//   }
// );

// // Add a response interceptor
// axios.interceptors.response.use(
//   (response) => {
//     // Process the response (e.g., handle data or status code)
//     console.log('Response Received:', response);
//     return response;
//   },
//   (error) => {
//     // Handle response errors (e.g., redirect on 401, log out on 403)
//     if (error.response && error.response.status === 401) {
//       // Example: Redirect to login page
//       window.location.href = '/login';
//     } else if (error.response && error.response.status === 403) {
//       // Example: Clear token and log out
//       localStorage.removeItem('authToken');
//       window.location.href = '/login';
//     }
//     console.error('Response Error:', error);
//     return Promise.reject(error);
//   }
// );

createApp(App)
    .use(VueAxios, axios)
    .use(ElementPlus)
    .mount('#app')