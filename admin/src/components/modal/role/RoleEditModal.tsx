import { Modal, Form, Input, message } from "antd";
import { useEffect, useState } from "react";
import { roleApi } from "../../../services/role.api";
import { Role, UpdateRoleDto } from "../../../types/role.type";

interface RoleEditModalProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
    role: Role;
}

const RoleEditModal = ({
    visible,
    onCancel,
    onSuccess,
    role,
}: RoleEditModalProps) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (visible && role) {
            form.setFieldsValue({
                roleName: role.roleName,
            });
        }
    }, [visible, role, form]);

    const handleSubmit = async (values: UpdateRoleDto) => {
        try {
            setLoading(true);
            await roleApi.updateRole(role.id, values);
            message.success("Role updated successfully");
            onSuccess();
        } catch (error) {
            console.error("Error updating role:", error);
            message.error("Failed to update role");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Modal
            title="Edit Role"
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

export default RoleEditModal;
