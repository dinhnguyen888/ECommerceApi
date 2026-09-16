import apiClient from "./APIConfig";

export const searchProducts = async (keyword) => {
    try {
        const response = await apiClient.get(
            `/products/search?keyword=${encodeURIComponent(keyword)}`
        );
        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to search products");
    }
};
