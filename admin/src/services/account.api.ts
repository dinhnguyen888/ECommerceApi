import { API_URL, axiosInstance } from "./apiConfig";
import {
    Account,
    AccountListResponse,
    CreateAccountDto,
    UpdateAccountDto,
} from "../types/account.type";
import { PaginationParams } from "../types/base.type";

const BASE_URL = `${API_URL}/Account`;

export const accountApi = {
    getAccounts: async ({
        page,
        pageSize,
    }: PaginationParams): Promise<AccountListResponse> => {
        const response = await axiosInstance.get<AccountListResponse>(
            `${BASE_URL}?page=${page}&pageSize=${pageSize}`
        );
        return response.data;
    },

    createAccount: async (data: CreateAccountDto): Promise<Account> => {
        const response = await axiosInstance.post<Account>(BASE_URL, data);
        return response.data;
    },

    updateAccount: async (
        accountId: string,
        data: UpdateAccountDto
    ): Promise<Account> => {
        const response = await axiosInstance.put<Account>(
            `${BASE_URL}/${accountId}`,
            {
                email: data.email,
                password: data.password,
                name: data.name,
                roleId: data.roleId,
            }
        );
        return response.data;
    },

    deleteAccount: async (accountId: string): Promise<void> => {
        await axiosInstance.delete(`${BASE_URL}/${accountId}`);
    },

    getAccountById: async (accountId: string): Promise<Account> => {
        const response = await axiosInstance.get<Account>(
            `${BASE_URL}/${accountId}`
        );
        return response.data;
    },
};
