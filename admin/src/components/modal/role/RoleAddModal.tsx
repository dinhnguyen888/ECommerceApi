import { Modal, Form, Input, message } from "antd";
import { useState } from "react";
import { roleApi } from "../../../services/role.api";
import { CreateRoleDto } from "../../../types/role.type";

interface RoleAddModalProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
}

const RoleAddModal = ({ visible, onCancel, onSuccess }: RoleAddModalProps) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (values: CreateRoleDto) => {
        try {
            setLoading(true);
            await roleApi.createRole(values);
            message.success("Role created successfully");
            form.resetFields();
            onSuccess();
        } catch (error) {
            console.error("Error creating role:", error);
            message.error("Failed to create role");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Modal
            title="Add New Role"
            open={visible}
            onCancel={onCancel}
            onOk={form.submit}
            confirmLoading={loading}
        >
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Form.Item
                    name="roleName"
                    label="Role Name"
                    rules={[
                        { required: true, message: "Please input role name" },
                    ]}
                >
                    <Input placeholder="Enter role name" />
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default RoleAddModal;
