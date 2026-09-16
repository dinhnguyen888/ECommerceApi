import { Button, Card, Tag } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import {
    Payment,
    PaymentResponse,
    PaymentGateway,
} from "../../types/payment.type";
import BaseTable from "../common/BaseTable";
import { ColumnsType } from "antd/es/table";
import { PaymentDeletePendingModal } from "../modal/payment/PaymentDeletePendingModal";

interface PaymentSectionProps {
    data: PaymentResponse;
    loading: boolean;
    onPageChange: (page: number, pageSize: number) => void;
    onDeleteOk: () => Promise<void>;
}

const gatewayColors: Record<PaymentGateway, string> = {
    VNPAY: "blue",
    PAYPAL: "green",
    MOMO: "purple",
};

const PaymentSection = ({
    data,
    loading,
    onPageChange,
    onDeleteOk,
}: PaymentSectionProps) => {
    const { showDeleteConfirm } = PaymentDeletePendingModal({
        onOk: onDeleteOk,
    });
    const columns: ColumnsType<Payment> = [
        {
            title: "Product",
            dataIndex: "productPay",
            key: "productPay",
            render: (text: string) => <strong>{text}</strong>,
        },
        {
            title: "Price",
            dataIndex: "productPrice",
            key: "productPrice",
            render: (price: number) => (
                <span>₫{price.toLocaleString("vi-VN")}</span>
            ),
            sorter: (a, b) => a.productPrice - b.productPrice,
        },
        {
            title: "Gateway",
            dataIndex: "paymentGateway",
            key: "paymentGateway",
            render: (gateway: PaymentGateway) => (
                <Tag color={gatewayColors[gateway]}>{gateway}</Tag>
            ),
            filters: [
                { text: "VNPAY", value: "VNPAY" },
                { text: "PayPal", value: "PAYPAL" },
                { text: "MoMo", value: "MOMO" },
            ],
            onFilter: (value, record) => record.paymentGateway === value,
        },
        {
            title: "Status",
            dataIndex: "paymentStatus",
            key: "paymentStatus",
            render: (status: boolean) => (
                <Tag color={status ? "success" : "warning"}>
                    {status ? "Completed" : "Pending"}
                </Tag>
            ),
            filters: [
                { text: "Completed", value: true },
                { text: "Pending", value: false },
            ],
            onFilter: (value, record) => record.paymentStatus === value,
        },
        {
            title: "Payment Date",
            dataIndex: "paymentDate",
            key: "paymentDate",
            render: (date: string) => date,
            sorter: (a, b) =>
                new Date(a.paymentDate).getTime() -
                new Date(b.paymentDate).getTime(),
        },
        {
            title: "User ID",
            dataIndex: "userId",
            key: "userId",
            ellipsis: true,
        },
    ];

    return (
        <Card
            title="Payments"
            extra={
                <Button
                    type="primary"
                    danger
                    icon={<DeleteOutlined />}
                    onClick={showDeleteConfirm}
                >
                    Delete Pending Payments
                </Button>
            }
        >
            <BaseTable<Payment, PaymentResponse>
                columns={columns}
                data={data}
                loading={loading}
                onPageChange={onPageChange}
                scroll={{ x: 1000 }}
                dataSource={data.payments}
            />
        </Card>
    );
};

export default PaymentSection;
