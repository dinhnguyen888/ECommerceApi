import { Table, Tooltip, Image, Space, Button } from "antd";
import { FaDollarSign, FaTrashAlt } from "react-icons/fa";
import { useAuth } from "../../contexts/AuthContext";
import { useCart } from "../../contexts/CartContext";
import { usePayment } from "../../contexts/PaymentContext";

const Cart = () => {
    const { userId } = useAuth();
    const { cartItems, removeFromCart, clearCart } = useCart();
    const { navigatePayment } = usePayment();

    console.log(cartItems);
    const handleBuyNow = (record) => {
        navigatePayment(record.id, {
            productPay: record.productName,
            productPrice: record.price,
        });
    };

    const handleRemoveFromCart = async (record) => {
        await removeFromCart(record.id);
    };

    const handleClearCart = async () => {
        await clearCart(userId);
    };

    const columns = [
        {
            title: "",
            dataIndex: "imageUrl",
            key: "imageUrl",
            width: 100,
            render: (text) => (
                <Image
                    src={text}
                    alt="Product"
                    width={60}
                    height={60}
                    style={{ objectFit: "cover", borderRadius: "8px" }}
                    preview={false}
                />
            ),
        },
        {
            title: "Tên",
            dataIndex: "productName",
            key: "productName",
            width: 200,
            render: (text) => (
                <Tooltip title={text}>
                    <span className="truncate block w-48">{text}</span>
                </Tooltip>
            ),
        },
        {
            title: "Giá",
            dataIndex: "price",
            key: "price",
            width: 100,
            render: (text) => (
                <span className="text-green-600 font-semibold">
                    {parseInt(text).toLocaleString()}₫
                </span>
            ),
        },
        {
            title: "Actions",
            key: "actions",
            width: 150,
            render: (_, record) => (
                <Space size="middle">
                    <Tooltip title="Mua">
                        <Button
                            type="link"
                            icon={<FaDollarSign className="text-lg" />}
                            onClick={() => handleBuyNow(record)}
                            className="text-blue-500 hover:text-blue-600 p-0 flex items-center"
                        />
                    </Tooltip>
                    <Tooltip title="Xóa">
                        <Button
                            type="link"
                            icon={<FaTrashAlt className="text-lg" />}
                            onClick={() => handleRemoveFromCart(record)}
                            className="text-red-500 hover:text-red-600 p-0 flex items-center"
                        />
                    </Tooltip>
                </Space>
            ),
        },
    ];

    return (
        <>
            <div
                style={{
                    display: "flex",
                    justifyContent: "right",
                    margin: "16px",
                }}
            >
                <Button
                    onClick={handleClearCart}
                    type="primary"
                    danger
                    className="flex items-center justify-center"
                >
                    Clear Cart
                </Button>
            </div>
            <Table
                columns={columns}
                dataSource={cartItems}
                pagination={false}
                bordered
                scroll={{ x: 600 }}
            />
        </>
    );
};

export default Cart;
