import apiClient from "./APIConfig";

export const changePassword = async (token, oldPassword, newPassword) => {
    try {
        const response = await apiClient.put(
            "/Profile/change-password",
            {
                oldPassword: oldPassword,
                newPassword: newPassword,
            },
            {
                headers: { Authorization: `Bearer ${token}` },
            }
        );
        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to change password");
    }
};
