import { PaginationMeta } from "./base.type";

export interface Account {
    id: string;
    email: string;
    name: string;
    roleName: string | null;
    pictureUrl: string | null;
}

export interface AccountListResponse extends PaginationMeta {
    accounts: Account[];
    totalAccounts: number;
}

export interface CreateAccountDto {
    email: string;
    password: string;
    name: string;
    roleId: number; // Changed to string type
}

export interface UpdateAccountDto {
    email: string;
    password: string;
    name: string;
    roleId: string;
}
