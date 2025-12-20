import { Card, Button, Tooltip, Tag } from "antd";
import { Role, Roles } from "../../types/role.type";
import BaseTable from "../common/BaseTable";
import { ColumnsType } from "antd/es/table";
import { EditOutlined, PlusOutlined, DeleteOutlined } from "@ant-design/icons";
import { PaginationMeta } from "../../types/base.type";

interface RoleSectionProps {
    roles: Roles;
    loading: boolean;
    onEdit: (role: Role) => void;
    onAdd: () => void;
    onDelete: (roleId: number) => void;
}

interface RoleTableData extends PaginationMeta {
    roles: Roles;
}

const RoleSection = ({
    roles,
    loading,
    onEdit,
    onAdd,
    onDelete,
}: RoleSectionProps) => {
    const columns: ColumnsType<Role> = [
        {
            title: "ID",
            dataIndex: "id",
            key: "id",
            width: 80,
        },
        {
            title: "Role Name",
            dataIndex: "roleName",
            key: "roleName",
            render: (roleName: string) => (
                <Tag color={roleName === "Admin" ? "red" : "blue"}>
                    {roleName}
                </Tag>
            ),
        },
        {
            title: "Actions",
            key: "actions",
            width: 120,
            render: (_, record) => (
                <Button.Group>
                    <Tooltip title="Edit">
                        <Button
                            icon={<EditOutlined />}
                            onClick={() => onEdit(record)}
                            type="text"
                        />
                    </Tooltip>
                    <Tooltip title="Delete">
                        <Button
                            icon={<DeleteOutlined />}
                            onClick={() => onDelete(record.id)}
                            type="text"
                            danger
                        />
                    </Tooltip>
                </Button.Group>
            ),
        },
    ];

    // Since roles don't have pagination, set default values
    const tableData: RoleTableData = {
        roles,
        currentPage: 1,
        pageSize: roles.length,
        totalPages: 1,
    };

    return (
        <Card
            title="Roles"
            extra={
                <Button type="primary" icon={<PlusOutlined />} onClick={onAdd}>
                    Add Role
                </Button>
            }
        >
            <BaseTable<Role, RoleTableData>
                columns={columns}
                data={tableData}
                dataSource={roles}
                loading={loading}
                rowKey="id"
            />
        </Card>
    );
};

export default RoleSection;
