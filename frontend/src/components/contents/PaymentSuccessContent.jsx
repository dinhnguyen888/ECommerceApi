import { Button, Result } from "antd";
import { CheckCircleOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

const PaymentSuccessContent = () => {
    const navigate = useNavigate();

    return (
        <div className="max-w-2xl mx-auto p-8">
            <Result
                icon={
                    <CheckCircleOutlined className="text-green-500 text-6xl" />
                }
                status="success"
                title={
                    <span className="text-2xl font-bold text-green-600">
                        Thanh toán thành công!
                    </span>
                }
                subTitle={
                    <div className="space-y-4 mt-4">
                        <p className="text-gray-600">
                            Cảm ơn bạn đã mua sản phẩm. Mình đã gởi đơn hàng cho
                            bạn qua email, nếu bạn không thấy vui lòng check
                            trong mục thư rác nhen!!
                        </p>
                    </div>
                }
                extra={[
                    <Button key="buy" onClick={() => navigate("/")}>
                        Về trang chủ
                    </Button>,
                ]}
            />
        </div>
    );
};

export default PaymentSuccessContent;
