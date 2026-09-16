import { Card, Row, Col, Table, Statistic } from "antd";
import { Payment } from "../../types/payment.type";
import { Product } from "../../types/product.type";
import { Account } from "../../types/account.type";
import dayjs from "dayjs";
import {
    ShoppingOutlined,
    UserOutlined,
    CheckCircleOutlined,
    DollarOutlined,
} from "@ant-design/icons";
import {
    ResponsiveContainer,
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    Pie,
    PieChart,
    Cell,
    Legend,
} from "recharts";

interface ChartData {
    name: string;
    successful: number;
    failed: number;
    total: number;
}

interface DashboardSectionProps {
    payments: Payment[];
    products: Product[];
    accounts: Account[];
    bestSellers: Array<{ productPay: string; count: number }>;
    salesData: ChartData[];
    loading: boolean;
}

const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#8884D8"];

const DashboardSection = ({
    payments,
    products,
    accounts,
    bestSellers,
    salesData,
    loading,
}: DashboardSectionProps) => {
    const totalSuccess = payments.filter((p) => p.paymentStatus).length;
    const totalAmount = payments
        .filter((p) => p.paymentStatus)
        .reduce((acc, p) => acc + p.productPrice, 0);

    const recentColumns = [
        {
            title: "Product",
            dataIndex: "productPay",
            key: "productPay",
        },
        {
            title: "Gateway",
            dataIndex: "paymentGateway",
            key: "paymentGateway",
        },
        {
            title: "Price",
            dataIndex: "productPrice",
            key: "productPrice",
            render: (price: number) => `₫${price.toLocaleString("vi-VN")}`,
        },
        {
            title: "Date",
            dataIndex: "paymentDate",
            key: "paymentDate",
            render: (date: string) => dayjs(date).format("DD/MM/YYYY HH:mm"),
        },
        {
            title: "Status",
            dataIndex: "paymentStatus",
            key: "paymentStatus",
            render: (status: boolean) => (
                <span style={{ color: status ? "#52c41a" : "#ff4d4f" }}>
                    {status ? "Success" : "Failed"}
                </span>
            ),
        },
    ];

    const sortedPayments = [...payments].sort(
        (a, b) =>
            dayjs(b.paymentDate).valueOf() - dayjs(a.paymentDate).valueOf()
    );

    return (
        <Row gutter={[16, 16]}>
            <Col xs={24} sm={12} md={6}>
                <Card>
                    <Statistic
                        title="Total Users"
                        value={accounts.length}
                        prefix={<UserOutlined />}
                    />
                </Card>
            </Col>
            <Col xs={24} sm={12} md={6}>
                <Card>
                    <Statistic
                        title="Total Products"
                        value={products.length}
                        prefix={<ShoppingOutlined />}
                    />
                </Card>
            </Col>
            <Col xs={24} sm={12} md={6}>
                <Card>
                    <Statistic
                        title="Successful Transactions"
                        value={totalSuccess}
                        prefix={<CheckCircleOutlined />}
                    />
                </Card>
            </Col>
            <Col xs={24} sm={12} md={6}>
                <Card>
                    <Statistic
                        title="Total Revenue"
                        value={totalAmount}
                        prefix={<DollarOutlined />}
                        formatter={(value) =>
                            `₫${value.toLocaleString("vi-VN")}`
                        }
                    />
                </Card>
            </Col>
            <Col xs={24} md={16}>
                <Card title="Monthly Transaction Statistics">
                    <div style={{ width: "100%", height: 350 }}>
                        <ResponsiveContainer>
                            <AreaChart data={salesData}>
                                <CartesianGrid strokeDasharray="3 3" />
                                <XAxis dataKey="name" />
                                <YAxis />
                                <Tooltip />
                                <Area
                                    type="monotone"
                                    dataKey="successful"
                                    name="Successful"
                                    stroke="#52c41a"
                                    fill="#52c41a"
                                    fillOpacity={0.3}
                                    strokeWidth={2}
                                />
                                <Area
                                    type="monotone"
                                    dataKey="failed"
                                    name="Failed"
                                    stroke="#ff4d4f"
                                    fill="#ff4d4f"
                                    fillOpacity={0.3}
                                    strokeWidth={2}
                                />
                            </AreaChart>
                        </ResponsiveContainer>
                    </div>
                </Card>
            </Col>
            <Col xs={24} md={8}>
                <Card title="Best Selling Products">
                    <div style={{ width: "100%", height: 350 }}>
                        <ResponsiveContainer>
                            <PieChart>
                                <Pie
                                    data={bestSellers}
                                    dataKey="count"
                                    nameKey="productPay"
                                    cx="50%"
                                    cy="50%"
                                    innerRadius={60}
                                    outerRadius={80}
                                    label
                                >
                                    {bestSellers.map((_, index) => (
                                        <Cell
                                            key={`cell-${index}`}
                                            fill={COLORS[index % COLORS.length]}
                                        />
                                    ))}
                                </Pie>
                                <Tooltip />
                                <Legend />
                            </PieChart>
                        </ResponsiveContainer>
                    </div>
                </Card>
            </Col>
            <Col span={24}>
                <Card title="Recent Transactions">
                    <Table
                        loading={loading}
                        dataSource={sortedPayments.slice(0, 5)}
                        columns={recentColumns}
                        rowKey="id"
                        pagination={false}
                    />
                </Card>
            </Col>
        </Row>
    );
};

export default DashboardSection;
