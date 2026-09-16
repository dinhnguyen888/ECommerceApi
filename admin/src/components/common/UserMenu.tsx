import { Button, Dropdown } from "antd";
import { UserOutlined, LogoutOutlined } from "@ant-design/icons";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

export const UserMenu = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/login");
    };

    const menuItems = [
        {
            key: "userInfo",
            label: (
                <div style={{ padding: "4px 0" }}>
                    <div style={{ fontWeight: 500 }}>{user?.userName}</div>
                    <div
                        style={{
                            color: "rgba(0, 0, 0, 0.45)",
                            fontSize: "12px",
                        }}
                    >
                        {user?.email}
                    </div>
                </div>
            ),
            disabled: true,
        },
        {
            type: "divider" as const,
        },
        {
            key: "logout",
            icon: <LogoutOutlined />,
            label: "Sign Out",
            onClick: handleLogout,
        },
    ];

    return (
        <div style={{ paddingRight: 24 }}>
            <Dropdown
                menu={{ items: menuItems }}
                trigger={["click"]}
                placement="bottomRight"
            >
                <Button
                    type="text"
                    icon={<UserOutlined />}
                    style={{
                        height: 40,
                        width: 40,
                        borderRadius: "50%",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                    }}
                />
            </Dropdown>
        </div>
    );
};

export default UserMenu;
