import { Modal, Form, Input, Select, message } from "antd";
import { useEffect, useState } from "react";
import { accountApi } from "../../../services/account.api";
import { Account, UpdateAccountDto } from "../../../types/account.type";

interface AccountEditModalProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
    accountId: string;
}

const AccountEditModal = ({
    visible,
    onCancel,
    onSuccess,
    accountId,
}: AccountEditModalProps) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [account, setAccount] = useState<Account | null>(null);

    useEffect(() => {
        if (visible && accountId) {
            fetchAccount();
        }
    }, [visible, accountId]);

    const fetchAccount = async () => {
        try {
            setLoading(true);
            const data = await accountApi.getAccountById(accountId);
            setAccount(data);
            form.setFieldsValue({
                email: data.email,
                name: data.name,
                roleId: data.roleName === "Admin" ? "1" : "2", // Set as string
            });
        } catch (error) {
            console.error("Error fetching account:", error);
            message.error("Failed to fetch account details");
            onCancel();
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (values: UpdateAccountDto) => {
        try {
            setLoading(true);
            await accountApi.updateAccount(accountId, {
                ...values,
                roleId: values.roleId, // Already a string from the form
            });
            message.success("Account updated successfully");
            onSuccess();
        } catch (error) {
            console.error("Error updating account:", error);
            message.error("Failed to update account");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Modal
            title="Edit Account"
            open={visible}
            onCancel={onCancel}
            onOk={form.submit}
            confirmLoading={loading}
        >
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Form.Item
                    name="email"
                    label="Email"
                    rules={[
                        { required: true, message: "Please input email" },
                        { type: "email", message: "Please input valid email" },
                    ]}
                >
                    <Input placeholder="Enter email" />
                </Form.Item>

                <Form.Item
                    name="password"
                    label="Password"
                    rules={[
                        {
                            min: 6,
                            message: "Password must be at least 6 characters",
                        },
                    ]}
                    extra="Leave empty to keep current password"
                >
                    <Input.Password placeholder="Enter new password (optional)" />
                </Form.Item>

                <Form.Item
                    name="name"
                    label="Name"
                    rules={[{ required: true, message: "Please input name" }]}
                >
                    <Input placeholder="Enter name" />
                </Form.Item>

                <Form.Item
                    name="roleId"
                    label="Role"
                    rules={[{ required: true, message: "Please select role" }]}
                >
                    <Select disabled={account?.roleName === "Admin"}>
                        <Select.Option value="1">Admin</Select.Option>
                        <Select.Option value="2">User</Select.Option>
                    </Select>
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default AccountEditModal;
