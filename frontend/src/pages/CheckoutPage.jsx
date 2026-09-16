import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { message } from "antd";
import { useAuth } from "../contexts/AuthContext";
import { usePayment } from "../contexts/PaymentContext";
import CheckoutContent from "../components/contents/CheckoutContent";
import BaseLayout from "../components/layout/BaseLayout";
import toast from "react-hot-toast";

const paymentMethods = [
    {
        value: "VNPAY",
        name: "VNPAY",
        logo: "/vnpay-logo.webp",
        isSandboxEnv: true,
    },
    {
        value: "PAYOS",
        name: "Ví điện tử PAYOS",
        logo: "/payos-logo.svg",
    },
    {
        value: "MOMO",
        name: "MoMo",
        logo: "/momo-logo.png",
        isSandboxEnv: true,
    },
    {
        value: "PAYPAL",
        name: "Ví điện tử PayPal",
        logo: "/paypal-logo.svg",
        isSandboxEnv: true,
    },
];

const CheckoutPage = () => {
    const { accessToken, userId, username, email } = useAuth();
    const {
        tempProductId,
        tempProductPay,
        tempPrice,
        initiatePayment,
        loading,
    } = usePayment();
    const [selectedGateway, setSelectedGateway] = useState("PAYOS");
    const navigate = useNavigate();

    useEffect(() => {
        if (!accessToken) {
            navigate("/dang-nhap", { replace: true });
        }
    }, [accessToken, navigate]);

    const handlePayment = async () => {
        try {
            if (!userId || !tempProductId || !tempProductPay || !tempPrice) {
                toast.error("Thiếu thông tin thanh toán!");
                return;
            }

            const paymentData = {
                productId: tempProductId,
                productName: tempProductPay,
                userId: userId,
                price: tempPrice,
            };

            await initiatePayment(paymentData, selectedGateway);
        } catch (error) {
            message.error("Không thể khởi tạo thanh toán: " + error.message);
        }
    };

    if (!accessToken) {
        return null;
    }

    return (
        <BaseLayout>
            <div className="container mx-auto py-8">
                <CheckoutContent
                    username={username}
                    email={email}
                    productName={tempProductPay}
                    productPrice={tempPrice}
                    selectedGateway={selectedGateway}
                    onGatewayChange={setSelectedGateway}
                    onPayment={handlePayment}
                    loading={loading}
                    paymentMethods={paymentMethods}
                />
                <h2 className="text-2xl font-bold my-7 text-center">
                    Lưu ý !!
                </h2>
                <div className="text-center text-lg">
                    {" "}
                    <p className="text-red-600 font-bold">
                        Tất cả các payment Gateway ở trên (trừ Payos do Payos
                        không có môi trường sandbox) đều chạy ở môi trường
                        sandbox
                    </p>
                    <div className="text-left mx-auto max-w-3xl mt-4 font-bold">
                        <p>
                            Đối với MOMO do là API key dùng chung nên đôi lúc
                            khi ấn xác nhận thanh toán sẽ bị lỗi, mong anh/chị
                            bỏ qua ạ!!!
                        </p>
                        <p>
                            Đối với Paypal tài khoản và mật khẩu dùng để test:
                        </p>
                        <p>Email: sb-vqd2937877603@personal.example.com</p>
                        <p>Pass: !M*iXKg4</p>
                    </div>
                </div>
            </div>
        </BaseLayout>
    );
};

export default CheckoutPage;
