import apiClient from "./APIConfig";

export const updateProfile = async (token, email, name) => {
    try {
        const response = await apiClient.put(
            "/Profile/update-profile",
            {
                email: email,
                name: name,
            },
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );

        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to update profile");
    }
};
