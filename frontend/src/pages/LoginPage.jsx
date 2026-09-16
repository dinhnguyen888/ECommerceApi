import AuthLayout from "../components/layout/AuthLayout";
import GenericForm from "../components/common/GenericForm";
import SocialLogin from "../components/common/SocialLogin";
import RegisterLink from "../components/common/AuthLink";
import { loginUser } from "../services/login.api";

import { useAuth } from "../contexts/AuthContext";

import toast from "react-hot-toast";

const LoginPage = () => {
    const { login } = useAuth();

    const handleGoogleLogin = () => {
        window.location.href =
            `${import.meta.env.VITE_API_URL}OAuth/google-login`;
    };

    const handleGithubLogin = () => {
        window.location.href =
            `${import.meta.env.VITE_API_URL}OAuth/github-login`;
    };

    const onFinish = async (values) => {
        try {
            var token = await loginUser(values.email, values.password);
            if (token) {
                login(token.accessToken);
                window.location.href = "/";
            }
        } catch (error) {
            if (error.response && error.response.status === 401) {
                toast.error("Vui lòng kiểm tra email hoặc mật khẩu.");
            } else {
                console.error("An unexpected error occurred:", error);
            }
        }
    };

    const onFinishFailed = (errorInfo) => {
        console.log("Form submission failed:", errorInfo);
    };

    const fields = [
        {
            label: "Email",
            name: "email",
            type: "text",
            required: true,
            message: "Please input your email!",
        },
        {
            label: "Password",
            name: "password",
            type: "password",
            required: true,
            message: "Please input your password!",
        },
    ];

    return (
        <AuthLayout>
            <h2 className="text-2xl font-bold text-center mb-6">Đăng nhập</h2>
            <GenericForm
                fields={fields}
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
                checkboxContent={"Remember me"}
            />
            <SocialLogin
                onGoogleClick={handleGoogleLogin}
                onGithubClick={handleGithubLogin}
            />

            <RegisterLink />
        </AuthLayout>
    );
};

export default LoginPage;
