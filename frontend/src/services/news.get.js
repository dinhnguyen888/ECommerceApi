import apiClient from "./APIConfig";

export const getNews = async (pageNumber = 1, pageSize = 5) => {
    try {
        const response = await apiClient.get(
            `/News?pageNumber=${pageNumber}&pageSize=${pageSize}`
        );
        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to fetch news");
    }
};
