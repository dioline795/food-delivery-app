import axiosClient from "./axiosClient";

// --- Auth ---
export const authApi = {
  login: (email, password) => axiosClient.post("/auth/login", { email, password }),
  register: (data) => axiosClient.post("/auth/register", data),
};

// --- Restaurants ---
export const restaurantApi = {
  getAll: () => axiosClient.get("/restaurants"),
  getById: (id) => axiosClient.get(`/restaurants/${id}`),
};

// --- Plats (menu d'un restaurant) ---
export const dishApi = {
  getByRestaurant: (restaurantId) => axiosClient.get(`/restaurants/${restaurantId}/dishes`),
};

// --- Commandes ---
export const orderApi = {
  create: (order) => axiosClient.post("/orders", order),
  getById: (id) => axiosClient.get(`/orders/${id}`),
  getMyOrders: () => axiosClient.get("/orders/my"),
  getAll: () => axiosClient.get("/orders"), // admin
  updateStatus: (id, status) => axiosClient.patch(`/orders/${id}/status`, { status }),
};
