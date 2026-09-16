import { Modal, Table, Tag } from "antd";
import { useEffect, useState } from "react";
import { CartItem } from "../../../types/cart.type";
import { cartApi } from "../../../services/cart.api";
import dayjs from "dayjs";
import { ColumnsType } from "antd/es/table";

interface ViewCartModalProps {
    visible: boolean;
    onCancel: () => void;
    userId: string;
}

const ViewCartModal = ({ visible, onCancel, userId }: ViewCartModalProps) => {
    const [loading, setLoading] = useState(false);
    const [cartItems, setCartItems] = useState<CartItem[]>([]);

    const fetchCart = async () => {
        try {
            setLoading(true);
            const data = await cartApi.getUserCart(userId);
            setCartItems(data);
        } catch (error) {
            console.error("Error fetching cart:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (visible && userId) {
            fetchCart();
        }
    }, [visible, userId]);

    const columns: ColumnsType<CartItem> = [
        {
            title: "Product",
            dataIndex: "imageUrl",
            key: "imageUrl",
            width: 120,
            render: (imageUrl: string, record) => (
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "10px",
                    }}
                >
                    <img
                        src={imageUrl}
                        alt={record.productName}
                        style={{
                            width: "60px",
                            height: "60px",
                            objectFit: "cover",
                            borderRadius: "4px",
                        }}
                    />
                    <span>{record.productName}</span>
                </div>
            ),
        },
        {
            title: "Price",
            dataIndex: "price",
            key: "price",
            width: 120,
            render: (price: number) => (
                <Tag color="blue">₫{price.toLocaleString("vi-VN")}</Tag>
            ),
        },
        {
            title: "Added At",
            dataIndex: "addToCartAt",
            key: "addToCartAt",
            width: 150,
            render: (date: string) => dayjs(date).format("DD/MM/YYYY HH:mm"),
        },
    ];

    return (
        <Modal
            title="User's Cart"
            open={visible}
            onCancel={onCancel}
            width={800}
            footer={null}
        >
            <Table<CartItem>
                columns={columns}
                dataSource={cartItems}
                loading={loading}
                rowKey="id"
                pagination={false}
                summary={(pageData) => {
                    const total = pageData.reduce(
                        (acc, item) => acc + item.price,
                        0
                    );
                    return (
                        <Table.Summary.Row>
                            <Table.Summary.Cell index={0}>
                                Total
                            </Table.Summary.Cell>
                            <Table.Summary.Cell index={1} colSpan={2}>
                                <Tag color="red">
                                    ₫{total.toLocaleString("vi-VN")}
                                </Tag>
                            </Table.Summary.Cell>
                        </Table.Summary.Row>
                    );
                }}
            />
        </Modal>
    );
};

export default ViewCartModal;
