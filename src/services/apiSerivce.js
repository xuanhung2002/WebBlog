import axios from "axios";

const API_BASE_URL = "https://localhost:7192/";

const apiService = axios.create({
    baseURL: API_BASE_URL,
    headers: {
      'Content-Type': 'application/json',
    },
    withCredentials: true,
  });

//   apiService.interceptors.request.use(
//     (config) => {
//     //   const token = localStorage.getItem('token');
//     //   if (token) {
//     //     config.headers.Authorization = `Bearer ${token}`;
//     //   }
//       return config;
//     },
//     (error) => Promise.reject(error)
//   );
  
  apiService.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response && error.response.status === 401) {
        console.error('Unauthorized - Redirect to login');
        throw new Error(error.response.data?.message || 'An error occurred');
      }
      return Promise.reject(error);
    }
  );

export const getData = (endpoint, params = {}) => apiService.get(endpoint, { params });
export const createData = (endpoint, data) => apiService.post(endpoint, data);
export const updateData = (endpoint, data) => apiService.put(endpoint, data);
export const deleteData = (endpoint, params = {}) => apiService.delete(endpoint, { params });

export default apiService;
