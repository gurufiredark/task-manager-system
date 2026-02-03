import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5239/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const taskService = {
  // Listar todas as tarefas com filtros opcionais
  getAllTasks: async (filters = {}) => {
    const params = new URLSearchParams();
    
    if (filters.status !== undefined && filters.status !== '') {
      params.append('status', filters.status);
    }
    if (filters.createdAfter) {
      params.append('createdAfter', filters.createdAfter);
    }
    if (filters.createdBefore) {
      params.append('createdBefore', filters.createdBefore);
    }
    if (filters.orderBy) {
      params.append('orderBy', filters.orderBy);
    }
    if (filters.orderDirection) {
      params.append('orderDirection', filters.orderDirection);
    }

    const response = await api.get(`/tasks?${params.toString()}`);
    return response.data;
  },

  getTaskById: async (id) => {
    const response = await api.get(`/tasks/${id}`);
    return response.data;
  },

  createTask: async (task) => {
    const response = await api.post('/tasks', task);
    return response.data;
  },

  updateTask: async (id, task) => {
    const response = await api.put(`/tasks/${id}`, task);
    return response.data;
  },

  deleteTask: async (id) => {
    await api.delete(`/tasks/${id}`);
  },
};

export default api;