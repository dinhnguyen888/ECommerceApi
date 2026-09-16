import apiClient from "./APIConfig";

export const viewPaymentHistory = async (token) => {
    try {
        const response = await apiClient.get("/Payment/view-payment-history", {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        throw new Error(
            error.response?.data?.Message || "Failed to fetch payment history"
        );
    }
};
