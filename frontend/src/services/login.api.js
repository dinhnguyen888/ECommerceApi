import apiClient from "./APIConfig";

export const loginUser = async (email, password) => {
    try {
        const response = await apiClient.post("/Auth/login", null, {
            params: {
                email,
                password,
            },
        });
        return response.data;
    } catch (error) {
        if (error.response) {
            if (error.response.status === 404) {
                console.error("Error 404: Not Found");
            } else if (error.response.status === 400) {
                console.error("Error 400: Bad Request");
            } else {
                console.error("Error logging in user:", error.response.status);
            }
        } else {
            console.error("Error logging in user:", error.message);
        }
        throw error;
    }
};
