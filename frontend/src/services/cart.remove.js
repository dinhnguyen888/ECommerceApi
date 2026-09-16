import apiClient from "./APIConfig";

export const removeFromCart = async (id) => {
    try {
        await apiClient.delete(`/Cart/${id}`);
    } catch (error) {
        throw new Error(
            error.response?.data || "Failed to remove item from cart"
        );
    }
};
