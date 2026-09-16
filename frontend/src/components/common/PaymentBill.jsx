import { useEffect } from "react";
import { Table, Tag, Alert, Button } from "antd";
import { ReloadOutlined } from "@ant-design/icons";
import { usePayment } from "../../contexts/PaymentContext";
import { useAuth } from "../../contexts/AuthContext";
import Loading from "./Loading";

const PaymentBill = () => {
    const { paymentHistory, loading, error, fetchPaymentHistory } =
        usePayment();
    const { isLoggedIn, accessToken } = useAuth();

    useEffect(() => {
        if (isLoggedIn && accessToken) {
            fetchPaymentHistory(accessToken);
        }
    }, [isLoggedIn, accessToken]);

    const handleRefresh = () => {
        if (isLoggedIn && accessToken) {
            fetchPaymentHistory(accessToken);
        }
    };

    const columns = [
        {
            title: "Sản phẩm",
            dataIndex: "productPay",
            key: "productPay",
        },
        {
            title: "Trạng thái",
            dataIndex: "paymentStatus",
            key: "paymentStatus",
            render: (status) => (
                <Tag color={status ? "green" : "red"}>
                    {status ? "Thanh toán thành công" : "Thanh toán thất bại"}
                </Tag>
            ),
        },
        {
            title: "Phương thức thanh toán",
            dataIndex: "paymentGateway",
            key: "paymentGateway",
            render: (gateway) => (
                <Tag color={getGatewayColor(gateway)}>{gateway}</Tag>
            ),
        },
        {
            title: "Số tiền",
            dataIndex: "productPrice",
            key: "productPrice",
            render: (price) => `${price.toLocaleString()}đ`,
            sorter: (a, b) => a.productPrice - b.productPrice,
        },
        {
            title: "Ngày thanh toán",
            dataIndex: "paymentDate",
            key: "paymentDate",
            sorter: (a, b) => new Date(a.paymentDate) - new Date(b.paymentDate),
        },
    ];

    const getGatewayColor = (gateway) => {
        switch (gateway) {
            case "PAYOS":
                return "blue";
            case "PAYPAL":
                return "green";
            case "VNPAY":
                return "red";
            case "MOMO":
                return "purple";
            default:
                return "default";
        }
    };

    if (loading) return <Loading />;

    if (!isLoggedIn) {
        return (
            <Alert
                message="Chưa đăng nhập"
                description="Vui lòng đăng nhập để xem lịch sử thanh toán"
                type="warning"
                showIcon
                className="mb-4"
            />
        );
    }

    return (
        <div className="container mx-auto p-4">
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-2xl font-bold">Lịch sử thanh toán</h2>
                <Button
                    icon={<ReloadOutlined />}
                    onClick={handleRefresh}
                    loading={loading}
                >
                    Làm mới
                </Button>
            </div>

            {error && (
                <Alert
                    message="Lỗi tải dữ liệu"
                    description={error}
                    type="error"
                    className="mb-4"
                    showIcon
                />
            )}

            <Table
                columns={columns}
                dataSource={paymentHistory.map((item, index) => ({
                    ...item,
                    key: index,
                }))}
                pagination={{
                    pageSize: 10,
                    showTotal: (total) => `Tổng ${total} giao dịch`,
                }}
                className="bg-white rounded-lg shadow"
                locale={{
                    emptyText: "Chưa có giao dịch nào",
                }}
            />
        </div>
    );
};

export default PaymentBill;
