import apiClient from "./APIConfig";

export const fetchProductDetail = async (productId) => {
    try {
        const response = await apiClient.get(`/products/detail/${productId}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching product details:", error);
        throw error;
    }
};
