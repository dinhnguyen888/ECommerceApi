import { Form, Input, Button } from "antd";
import { MailOutlined, LockOutlined } from "@ant-design/icons";

export interface LoginFormData {
    email: string;
    password: string;
}

interface LoginSectionProps {
    onFinish: (values: LoginFormData) => Promise<void>;
    isLoading?: boolean;
}

export const LoginSection = ({ onFinish, isLoading }: LoginSectionProps) => {
    const [form] = Form.useForm();

    return (
        <>
            <div style={{ textAlign: "center", marginBottom: "32px" }}>
                <h1
                    style={{
                        fontSize: "32px",
                        margin: "0 0 12px",
                        fontWeight: 600,
                        background:
                            "linear-gradient(135deg, #1890ff 0%, #722ed1 100%)",
                        WebkitBackgroundClip: "text",
                        WebkitTextFillColor: "transparent",
                    }}
                >
                    Welcome Back
                </h1>
                <p
                    style={{
                        color: "rgba(0, 0, 0, 0.45)",
                        fontSize: "16px",
                        margin: 0,
                    }}
                >
                    Sign in to your account
                </p>
            </div>
            <Form
                form={form}
                name="login"
                onFinish={onFinish}
                autoComplete="off"
                layout="vertical"
                size="large"
                style={{
                    gap: "20px",
                    display: "flex",
                    flexDirection: "column",
                }}
            >
                <Form.Item
                    name="email"
                    rules={[
                        {
                            required: true,
                            message: "Please input your email!",
                        },
                        {
                            type: "email",
                            message: "Please enter a valid email!",
                        },
                    ]}
                >
                    <Input
                        prefix={<MailOutlined style={{ color: "#1890ff" }} />}
                        placeholder="Email"
                        size="large"
                        style={{
                            borderRadius: "8px",
                            height: "45px",
                        }}
                    />
                </Form.Item>

                <Form.Item
                    name="password"
                    rules={[
                        {
                            required: true,
                            message: "Please input your password!",
                        },
                    ]}
                >
                    <Input.Password
                        prefix={<LockOutlined style={{ color: "#1890ff" }} />}
                        placeholder="Password"
                        size="large"
                        style={{
                            borderRadius: "8px",
                            height: "45px",
                        }}
                    />
                </Form.Item>

                <Form.Item style={{ marginTop: "12px" }}>
                    <Button
                        type="primary"
                        htmlType="submit"
                        block
                        size="large"
                        loading={isLoading}
                        style={{
                            height: "45px",
                            borderRadius: "8px",
                            fontSize: "16px",
                            fontWeight: 500,
                            background:
                                "linear-gradient(135deg, #1890ff 0%, #722ed1 100%)",
                            border: "none",
                        }}
                    >
                        Sign In
                    </Button>
                </Form.Item>
            </Form>
        </>
    );
};

export default LoginSection;
