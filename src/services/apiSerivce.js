import axios from "axios";
import { refreshAccessToken } from "./authService";
import { ROUTES } from "../constants/routes";
import Cookies from "js-cookie";

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
    async (error) => {
      const originalRequest = error.config
      if (error.response?.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        // refresh token
        try {
          await refreshAccessToken();
          return axios.request(error.config);
        } catch (refreshError) {
          Cookies.remove("isLogged");         
          window.location.href = ROUTES.LOGIN;
          return Promise.reject(refreshError);
        }             
      }
      return Promise.reject(error);
    }
  );


export const getData = (endpoint, params = {}) => apiService.get(endpoint, { params });
export const createData = (endpoint, data) => apiService.post(endpoint, data);
export const updateData = (endpoint, data) => apiService.put(endpoint, data);
export const deleteData = (endpoint, params = {}) => apiService.delete(endpoint, { params });

export default apiService;
