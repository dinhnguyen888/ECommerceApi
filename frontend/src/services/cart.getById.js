import apiClient from "./APIConfig";

export const getCartByUserId = async (userId) => {
    try {
        const response = await apiClient.get(`/Cart/${userId}`);
        console.log(response.data);
        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to fetch cart items");
    }
};
