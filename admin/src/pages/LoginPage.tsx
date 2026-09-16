import { message } from "antd";
import { useNavigate } from "react-router-dom";
import AuthLayout from "../components/layout/AuthLayout";
import {
    LoginSection,
    LoginFormData,
} from "../components/section/LoginSection";
import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import { authApi } from "../services/auth.api";

export const LoginPage = () => {
    const navigate = useNavigate();
    const { login } = useAuth();
    const [isLoading, setIsLoading] = useState(false);

    const handleLogin = async (values: LoginFormData) => {
        setIsLoading(true);
        try {
            const response = await authApi.login(values);

            login({
                accessToken: response.accessToken,
                refreshToken: response.refreshToken,
            });

            message.success("Login successful!");
            navigate("/dashboard");
        } catch (err) {
            console.error("Login error:", err);
            message.error("Login failed! Please check your credentials.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <AuthLayout>
            <LoginSection onFinish={handleLogin} isLoading={isLoading} />
        </AuthLayout>
    );
};

export default LoginPage;
