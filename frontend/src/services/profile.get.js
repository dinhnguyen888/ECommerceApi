import apiClient from "./APIConfig";

export const getProfile = async (token) => {
    try {
        const response = await apiClient.get("/Profile", {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to fetch profile");
    }
};
