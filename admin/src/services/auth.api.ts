import { LoginRequest, LoginResponse } from "../types/auth.type";
import { axiosInstance } from "./apiConfig";

export const authApi = {
    login: async (data: LoginRequest): Promise<LoginResponse> => {
        const response = await axiosInstance.post("/Auth/admin-login", null, {
            params: {
                email: data.email,
                password: data.password,
            },
        });
        return response.data;
    },
};
