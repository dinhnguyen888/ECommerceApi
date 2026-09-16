import { Card, Button, Tooltip, Tag } from "antd";
import { Account, AccountListResponse } from "../../types/account.type";
import BaseTable from "../common/BaseTable";
import { ColumnsType } from "antd/es/table";
import {
    EditOutlined,
    PlusOutlined,
    DeleteOutlined,
    ShoppingCartOutlined,
} from "@ant-design/icons";
import { useState } from "react";
import ViewCartModal from "../modal/account/ViewCartModal";

interface AccountSectionProps {
    data: AccountListResponse;
    loading: boolean;
    onPageChange: (page: number, pageSize: number) => void;
    onEdit: (id: string) => void;
    onAdd: () => void;
    onDelete: (id: string) => void;
}

const AccountSection = ({
    data,
    loading,
    onPageChange,
    onEdit,
    onAdd,
    onDelete,
}: AccountSectionProps) => {
    const [viewCartModalVisible, setViewCartModalVisible] = useState(false);
    const [selectedUserId, setSelectedUserId] = useState<string | null>(null);

    const handleViewCart = (userId: string) => {
        setSelectedUserId(userId);
        setViewCartModalVisible(true);
    };

    const columns: ColumnsType<Account> = [
        {
            title: "Name",
            dataIndex: "name",
            key: "name",
            render: (text: string) => <strong>{text}</strong>,
        },
        {
            title: "Email",
            dataIndex: "email",
            key: "email",
        },
        {
            title: "Role",
            dataIndex: "roleName",
            key: "roleName",
            render: (role: string) => (
                <Tag color={role === "Admin" ? "red" : "blue"}>{role}</Tag>
            ),
            filters: [
                { text: "Admin", value: "Admin" },
                { text: "User", value: "User" },
            ],
            onFilter: (value, record) => record.roleName === value,
        },
        {
            title: "Actions",
            key: "actions",
            width: 180,
            render: (_, record) => (
                <Button.Group>
                    <Tooltip title="View Cart">
                        <Button
                            icon={<ShoppingCartOutlined />}
                            onClick={() => handleViewCart(record.id)}
                            type="text"
                        />
                    </Tooltip>
                    <Tooltip title="Edit">
                        <Button
                            icon={<EditOutlined />}
                            onClick={() => onEdit(record.id)}
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

    return (
        <>
            <Card
                title="Accounts"
                extra={
                    <Button
                        type="primary"
                        icon={<PlusOutlined />}
                        onClick={onAdd}
                    >
                        Add Account
                    </Button>
                }
            >
                <BaseTable<Account, AccountListResponse>
                    columns={columns}
                    data={data}
                    loading={loading}
                    onPageChange={onPageChange}
                    scroll={{ x: 800 }}
                    dataSource={data.accounts}
                />
            </Card>
            {selectedUserId && (
                <ViewCartModal
                    visible={viewCartModalVisible}
                    onCancel={() => {
                        setViewCartModalVisible(false);
                        setSelectedUserId(null);
                    }}
                    userId={selectedUserId}
                />
            )}
        </>
    );
};

export default AccountSection;
