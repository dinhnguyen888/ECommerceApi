import { Layout } from "antd";
import { ReactNode } from "react";

const { Content } = Layout;

interface AuthLayoutProps {
    children: ReactNode;
}

export const AuthLayout = ({ children }: AuthLayoutProps) => {
    return (
        <Layout style={{ height: "100%" }}>
            <Content
                style={{
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    padding: "80px 20px",
                    background: "#ffffff",
                }}
            >
                <div
                    style={{
                        width: "100%",
                        maxWidth: "400px",
                        padding: "40px",
                        backgroundColor: "#fff",
                        borderRadius: "20px",
                        boxShadow: "0 10px 30px rgba(0,0,0,0.08)",
                    }}
                >
                    {children}
                </div>
            </Content>
        </Layout>
    );
};

export default AuthLayout;
