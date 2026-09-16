import { Layout, Menu } from "antd";
import { useLocation, useNavigate } from "react-router-dom";
import { menuItems } from "../../config/menuItems";
import { createElement } from "react";

const { Sider } = Layout;

interface SidebarProps {
    collapsed: boolean;
}

const styles = {
    sider: {
        overflow: "auto",
        height: "100vh",
        position: "fixed",
        left: 0,
        top: 0,
        bottom: 0,
        boxShadow: "2px 0 8px rgba(0,0,0,0.1)",
        zIndex: 2,
    } as const,
    logo: {
        height: "40px",
        margin: "16px",
        background: "rgba(255, 255, 255, 0.1)",
        borderRadius: "8px",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        color: "#fff",
        fontSize: "18px",
        fontWeight: 600,
        letterSpacing: "0.5px",
        boxShadow: "0 2px 8px rgba(0,0,0,0.15) inset",
    } as const,
    menu: {
        border: "none",
    } as const,
};

export const Sidebar = ({ collapsed }: SidebarProps) => {
    const location = useLocation();
    const navigate = useNavigate();

    const items = menuItems.map((item) => ({
        ...item,
        icon: createElement(item.icon),
    }));

    // Get the current selected key based on the pathname
    const selectedKey =
        menuItems.find(
            (item) =>
                location.pathname === item.path ||
                (location.pathname === "/" && item.path === "/dashboard")
        )?.key || "";

    return (
        <Sider
            trigger={null}
            collapsible
            collapsed={collapsed}
            style={styles.sider}
        >
            <div style={styles.logo}>{collapsed ? "🚀" : "Dashboard"}</div>
            <Menu
                theme="dark"
                mode="inline"
                inlineCollapsed={collapsed}
                selectedKeys={[selectedKey]}
                items={items}
                onClick={({ key }) => {
                    const menuItem = menuItems.find((item) => item.key === key);
                    if (menuItem) {
                        navigate(menuItem.path);
                    }
                    if (window.innerWidth <= 768) {
                        // setCollapsed(true); // This would need to be handled by the parent
                    }
                }}
                style={styles.menu}
            />
        </Sider>
    );
};

export default Sidebar;
