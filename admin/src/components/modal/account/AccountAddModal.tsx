import { Modal, Form, Input, Select, message } from "antd";
import { useState } from "react";
import { accountApi } from "../../../services/account.api";
import { CreateAccountDto } from "../../../types/account.type";

interface AccountAddModalProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
}

const AccountAddModal = ({
    visible,
    onCancel,
    onSuccess,
}: AccountAddModalProps) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (values: CreateAccountDto) => {
        try {
            setLoading(true);
            // Send roleId directly as string
            await accountApi.createAccount(values);
            message.success("Account created successfully");
            form.resetFields();
            onSuccess();
        } catch (error) {
            console.error("Error creating account:", error);
            message.error("Failed to create account");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Modal
            title="Add New Account"
            open={visible}
            onCancel={onCancel}
            onOk={form.submit}
            confirmLoading={loading}
        >
            <Form
                form={form}
                layout="vertical"
                onFinish={handleSubmit}
                initialValues={{ roleId: "2" }} // Default to User role as string
            >
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
                        { required: true, message: "Please input password" },
                        {
                            min: 6,
                            message: "Password must be at least 6 characters",
                        },
                    ]}
                >
                    <Input.Password placeholder="Enter password" />
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
                    <Select>
                        <Select.Option value="1">Admin</Select.Option>
                        <Select.Option value="2">User</Select.Option>
                    </Select>
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default AccountAddModal;
