import AuthLayout from "../components/layout/AuthLayout";
import GenericForm from "../components/common/GenericForm";
import { registerUser } from "../services/register.api";
import toast from "react-hot-toast";

const RegisterPage = () => {
    const onFinish = async (values) => {
        try {
            await registerUser(values);
            window.location.href = "/dang-nhap";
        } catch (error) {
            toast.error("Failed to register! Please try again later.");
            console.error("Registration failed:", error);
        }
    };

    const onFinishFailed = () => {
        toast.error("Please fill in all required fields!");
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
            label: "Name",
            name: "name",
            type: "text",
            required: true,
            message: "Please input your name!",
        },
        {
            label: "Password",
            name: "password",
            type: "password",
            required: true,
            message: "Please input your password!",
        },
        {
            label: "Confirm Password",
            name: "confirmPassword",
            type: "password",
            required: true,
            message: "Please confirm your password!",
        },
    ];

    return (
        <AuthLayout>
            <h2 className="text-2xl font-bold text-center mb-6">Đăng ký</h2>
            <GenericForm
                fields={fields}
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
            />
            <p
                className="text-left cursor-pointer text-red-500 "
                onClick={() => (window.location.href = "/dang-nhap")}
            >
                Đã có tài khoản? Đăng nhập thôi
            </p>
        </AuthLayout>
    );
};

export default RegisterPage;
