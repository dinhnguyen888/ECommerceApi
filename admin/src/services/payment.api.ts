import { PaginationParams, PaymentResponse } from "../types/payment.type";
import { axiosInstance } from "./apiConfig";

export const paymentApi = {
    getPayments: async (params: PaginationParams): Promise<PaymentResponse> => {
        const response = await axiosInstance.get("/Payment", {
            params: {
                page: params.page,
                pageSize: params.pageSize,
            },
        });
        return response.data;
    },

    deletePendingPayment: async (): Promise<void> => {
        try {
            const response = await axiosInstance.delete(
                "/Payment/pending-payment"
            );
            console.log("Delete pending payments response:", response);
            // Accept both 200 and 204 as successful responses
            if (response.status !== 200 && response.status !== 204) {
                throw new Error(`Server returned status ${response.status}`);
            }
        } catch (error) {
            console.error("Error in deletePendingPayment:", error);
            throw error; // Re-throw to handle in the component
        }
    },
};

export default paymentApi;
