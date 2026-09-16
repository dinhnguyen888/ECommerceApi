import apiClient from "./APIConfig";

export const getMultipleProductsByTags = async (tagRequests) => {
    try {
        const response = await apiClient.post(
            "/Products/multiple-product-by-tags",
            tagRequests
        );
        console.log("API Response:", response.data);
        return response.data;
    } catch (error) {
        console.error("API Error:", error.response?.data || error);
        throw new Error(
            error.response?.data?.message || "Failed to fetch products"
        );
    }
};
