import apiClient from "./APIConfig";

export const clearCart = async (userId) => {
    try {
        await apiClient.delete(`/Cart/clear/${userId}`);
    } catch (error) {
        throw new Error(error.response?.data || "Failed to clear cart");
    }
};
