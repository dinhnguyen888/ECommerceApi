import apiClient from "./APIConfig";

export const getProductsByTag = async (tag, page = 1, pageSize = 6) => {
    try {
        const response = await apiClient.get(
            `/Products/tag?tag=${encodeURIComponent(tag)}&page=${page}&pageSize=${pageSize}`
        );
        return response.data;
    } catch (error) {
        throw new Error(
            error.response?.data || "Failed to fetch products by tag"
        );
    }
};
