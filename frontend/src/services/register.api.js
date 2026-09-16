import apiClient from "./APIConfig";

export const registerUser = async (userData) => {
    try {
        const response = await apiClient.post("/Auth/register", userData);
        return response.data;
    } catch (error) {
        console.error("Error registering user:", error);
        throw error;
    }
};
