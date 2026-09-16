import { useState, useEffect } from "react";
import { message, Modal } from "antd";
import BaseLayout from "../components/layout/BaseLayout";
import RoleSection from "../components/section/RoleSection";
import { Role, Roles } from "../types/role.type";
import { roleApi } from "../services/role.api";
import RoleAddModal from "../components/modal/role/RoleAddModal";
import RoleEditModal from "../components/modal/role/RoleEditModal";

export const RolePage = () => {
    const [loading, setLoading] = useState(false);
    const [roles, setRoles] = useState<Roles>([]);
    const [addModalVisible, setAddModalVisible] = useState(false);
    const [editModalVisible, setEditModalVisible] = useState(false);
    const [selectedRole, setSelectedRole] = useState<Role | null>(null);

    const fetchRoles = async () => {
        try {
            setLoading(true);
            const response = await roleApi.getRoles();
            setRoles(response);
        } catch (error) {
            console.error("Error fetching roles:", error);
            message.error("Failed to fetch roles");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchRoles();
    }, []);

    const handleAdd = () => {
        setAddModalVisible(true);
    };

    const handleEdit = (role: Role) => {
        setSelectedRole(role);
        setEditModalVisible(true);
    };

    const handleDelete = (roleId: number) => {
        Modal.confirm({
            title: "Are you sure you want to delete this role?",
            content: "This action cannot be undone.",
            okText: "Yes, delete",
            okType: "danger",
            cancelText: "No, cancel",
            onOk: async () => {
                try {
                    await roleApi.deleteRole(roleId);
                    message.success("Role deleted successfully");
                    fetchRoles();
                } catch (error) {
                    console.error("Error deleting role:", error);
                    message.error("Failed to delete role");
                }
            },
        });
    };

    const handleModalSuccess = () => {
        setAddModalVisible(false);
        setEditModalVisible(false);
        setSelectedRole(null);
        fetchRoles();
    };

    return (
        <BaseLayout>
            <RoleSection
                roles={roles}
                loading={loading}
                onEdit={handleEdit}
                onAdd={handleAdd}
                onDelete={handleDelete}
            />
            <RoleAddModal
                visible={addModalVisible}
                onCancel={() => setAddModalVisible(false)}
                onSuccess={handleModalSuccess}
            />
            {selectedRole && (
                <RoleEditModal
                    visible={editModalVisible}
                    role={selectedRole}
                    onCancel={() => {
                        setEditModalVisible(false);
                        setSelectedRole(null);
                    }}
                    onSuccess={handleModalSuccess}
                />
            )}
        </BaseLayout>
    );
};

export default RolePage;
