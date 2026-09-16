import { useState, useEffect } from "react";
import { message, Modal } from "antd";
import BaseLayout from "../components/layout/BaseLayout";
import AccountSection from "../components/section/AccountSection";
import { AccountListResponse } from "../types/account.type";
import { PaginationParams } from "../types/base.type";
import { accountApi } from "../services/account.api";
import AccountAddModal from "../components/modal/account/AccountAddModal";
import AccountEditModal from "../components/modal/account/AccountEditModal";

const initialPagination: PaginationParams = {
    page: 1,
    pageSize: 10,
};

export const AccountPage = () => {
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<AccountListResponse>({
        currentPage: 1,
        pageSize: 10,
        totalAccounts: 0,
        totalPages: 0,
        accounts: [],
    });
    const [addModalVisible, setAddModalVisible] = useState(false);
    const [editModalVisible, setEditModalVisible] = useState(false);
    const [selectedAccountId, setSelectedAccountId] = useState<string | null>(
        null
    );

    const fetchAccounts = async (params: PaginationParams) => {
        try {
            setLoading(true);
            const response = await accountApi.getAccounts(params);
            setData(response);
        } catch (error) {
            console.error("Error fetching accounts:", error);
            message.error("Failed to fetch accounts");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAccounts(initialPagination);
    }, []);

    const handlePageChange = (page: number, pageSize: number) => {
        fetchAccounts({ page, pageSize });
    };

    const handleEdit = (id: string) => {
        setSelectedAccountId(id);
        setEditModalVisible(true);
    };

    const handleAdd = () => {
        setAddModalVisible(true);
    };

    const handleDelete = (id: string) => {
        Modal.confirm({
            title: "Are you sure you want to delete this account?",
            content: "This action cannot be undone.",
            okText: "Yes, delete",
            okType: "danger",
            cancelText: "No, cancel",
            onOk: async () => {
                try {
                    await accountApi.deleteAccount(id);
                    message.success("Account deleted successfully");
                    fetchAccounts({
                        page: data.currentPage,
                        pageSize: data.pageSize,
                    });
                } catch (error) {
                    console.error("Error deleting account:", error);
                    message.error("Failed to delete account");
                }
            },
        });
    };

    const handleModalSuccess = () => {
        setAddModalVisible(false);
        setEditModalVisible(false);
        setSelectedAccountId(null);
        fetchAccounts({
            page: data.currentPage,
            pageSize: data.pageSize,
        });
    };

    return (
        <BaseLayout>
            <AccountSection
                data={data}
                loading={loading}
                onPageChange={handlePageChange}
                onEdit={handleEdit}
                onAdd={handleAdd}
                onDelete={handleDelete}
            />
            <AccountAddModal
                visible={addModalVisible}
                onCancel={() => setAddModalVisible(false)}
                onSuccess={handleModalSuccess}
            />
            {selectedAccountId && (
                <AccountEditModal
                    visible={editModalVisible}
                    accountId={selectedAccountId}
                    onCancel={() => {
                        setEditModalVisible(false);
                        setSelectedAccountId(null);
                    }}
                    onSuccess={handleModalSuccess}
                />
            )}
        </BaseLayout>
    );
};

export default AccountPage;
