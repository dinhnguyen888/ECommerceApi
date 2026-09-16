import { API_URL, axiosInstance } from "./apiConfig";
import { Roles, Role, CreateRoleDto, UpdateRoleDto } from "../types/role.type";

const BASE_URL = `${API_URL}/Role`;

export const roleApi = {
    getRoles: async (): Promise<Roles> => {
        const response = await axiosInstance.get<Roles>(BASE_URL);
        return response.data;
    },

    createRole: async (data: CreateRoleDto): Promise<Role> => {
        const response = await axiosInstance.post<Role>(BASE_URL, data);
        return response.data;
    },

    updateRole: async (roleId: number, data: UpdateRoleDto): Promise<Role> => {
        const response = await axiosInstance.put<Role>(
            `${BASE_URL}/${roleId}`,
            data
        );
        return response.data;
    },

    deleteRole: async (roleId: number): Promise<void> => {
        await axiosInstance.delete(`${BASE_URL}/${roleId}`);
    },
};
