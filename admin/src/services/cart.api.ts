import { API_URL, axiosInstance } from "./apiConfig";
import { CartItems } from "../types/cart.type";

const BASE_URL = `${API_URL}/Cart`;

export const cartApi = {
    getUserCart: async (userId: string): Promise<CartItems> => {
        const response = await axiosInstance.get<CartItems>(
            `${BASE_URL}/${userId}`
        );
        return response.data;
    },
};
