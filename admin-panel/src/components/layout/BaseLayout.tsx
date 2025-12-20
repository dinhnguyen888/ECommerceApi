import { useState } from "react";
import { Layout, Button, theme } from "antd";
import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons";
import type { ReactNode, CSSProperties } from "react";
import Sidebar from "../common/Sidebar";
import UserMenu from "../common/UserMenu";

const { Header, Content, Footer } = Layout;

interface BaseLayoutProps {
    children: ReactNode;
}

const styles = {
    layout: {
        minHeight: "100vh",
    } as CSSProperties,
    innerLayout: (collapsed: boolean): CSSProperties => ({
        marginLeft: collapsed ? 80 : 200,
        transition: "all 0.2s",
        minHeight: "100vh",
        background: "#f5f5f5",
    }),
    header: {
        padding: "0 16px",
        position: "sticky",
        top: 0,
        zIndex: 1,
        width: "100%",
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
        boxShadow: "0 2px 8px rgba(0,0,0,0.06)",
        height: 64,
    } as CSSProperties,
    toggleButton: {
        fontSize: "16px",
        width: 48,
        height: 48,
        margin: "8px 0",
    } as CSSProperties,
    content: (
        colorBgContainer: string,
        borderRadiusLG: number
    ): CSSProperties => ({
        margin: "24px",
        padding: "32px",
        background: colorBgContainer,
        borderRadius: borderRadiusLG,
        minHeight: "calc(100vh - 140px)",
        boxShadow: "0 2px 8px rgba(0,0,0,0.06)",
        display: "flex",
        flexDirection: "column",
        flexGrow: 1,
    }),
    footer: {
        textAlign: "center",
        padding: "16px",
        background: "transparent",
        color: "rgba(0, 0, 0, 0.45)",
        fontSize: "14px",
        margin: "0 24px 24px",
    } as CSSProperties,
};

export const BaseLayout = ({ children }: BaseLayoutProps) => {
    const [collapsed, setCollapsed] = useState(false);
    const {
        token: { colorBgContainer, borderRadiusLG },
    } = theme.useToken();

    return (
        <Layout style={styles.layout}>
            <Sidebar collapsed={collapsed} />
            <Layout style={styles.innerLayout(collapsed)}>
                <Header
                    style={{
                        ...styles.header,
                        background: colorBgContainer,
                    }}
                >
                    <div style={{ display: "flex", alignItems: "center" }}>
                        <Button
                            type="text"
                            icon={
                                collapsed ? (
                                    <MenuUnfoldOutlined />
                                ) : (
                                    <MenuFoldOutlined />
                                )
                            }
                            onClick={() => setCollapsed(!collapsed)}
                            style={styles.toggleButton}
                        />
                    </div>
                    <UserMenu />
                </Header>
                <Content
                    style={styles.content(colorBgContainer, borderRadiusLG)}
                >
                    {children}
                </Content>
                <Footer style={styles.footer}>
                    @nguyenphucdinh made with AI
                </Footer>
            </Layout>
        </Layout>
    );
};

export default BaseLayout;
