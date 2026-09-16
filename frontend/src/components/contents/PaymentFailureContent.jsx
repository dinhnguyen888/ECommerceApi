import { Button, Result } from "antd";
import { CloseCircleOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

const PaymentFailureContent = () => {
    const navigate = useNavigate();

    return (
        <div className="max-w-2xl mx-auto p-8">
            <Result
                icon={<CloseCircleOutlined className="text-red-500 text-6xl" />}
                status="error"
                title={
                    <span className="text-2xl font-bold text-red-600">
                        Thanh toán thất bại!
                    </span>
                }
                subTitle={
                    <div className="space-y-4 mt-4">
                        <p className="text-gray-600">
                            Rất tiếc, giao dịch của bạn không thể hoàn thành.
                            Vui lòng thử lại hoặc chọn phương thức thanh toán
                            khác.
                        </p>
                    </div>
                }
                extra={[
                    <Button
                        key="retry"
                        type="primary"
                        onClick={() => navigate(-1)}
                        className="mr-4"
                    >
                        Thử lại
                    </Button>,
                    <Button
                        key="contact"
                        onClick={() =>
                            (window.location.href =
                                "https://www.facebook.com/nguyenphucdinh.hosting/")
                        }
                    >
                        Liên hệ hỗ trợ
                    </Button>,
                ]}
            />
        </div>
    );
};

export default PaymentFailureContent;
