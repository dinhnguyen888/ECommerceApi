export interface Role {
    id: number;
    roleName: string;
}

export type Roles = Role[];

export interface CreateRoleDto {
    roleName: string;
}

export interface UpdateRoleDto {
    roleName: string;
}
