import { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { message } from "antd";
import Loading from "../../components/common/Loading";

const OAuthCallback = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const { login } = useAuth();

    useEffect(() => {
        const accessToken = searchParams.get("accessToken");
        const error = searchParams.get("error");

        if (error) {
            message.error("Đăng nhập thất bại: " + error);
            navigate("/dang-nhap");
            return;
        }

        if (accessToken) {
            try {
                login(accessToken);
                message.success("Đăng nhập thành công");
                navigate("/");
            } catch (err) {
                console.error("Error processing access token:", err);
                message.error("Có lỗi xảy ra khi xử lý đăng nhập");
                navigate("/dang-nhap");
            }
        } else {
            message.error("Không nhận được access token");
            navigate("/dang-nhap");
        }
    }, [searchParams, login, navigate]);

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50">
            <div className="text-center">
                <Loading />
                <p className="mt-4 text-gray-600">Đang xử lý đăng nhập...</p>
            </div>
        </div>
    );
};

export default OAuthCallback;
