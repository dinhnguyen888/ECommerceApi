import apiClient from "./APIConfig";

export const getBanner = async () => {
    try {
        const response = await apiClient.get("/Banner/", {});
        return response.data;
    } catch (error) {
        throw new Error(
            error.response?.data?.Message || "Failed to fetch banner"
        );
    }
};
