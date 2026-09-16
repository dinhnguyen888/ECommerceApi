import { useState, useEffect } from "react";
import BaseLayout from "../components/layout/BaseLayout";
import DashboardSection from "../components/section/DashboardSection";
import { Payment } from "../types/payment.type";
import { Product } from "../types/product.type";
import { Account } from "../types/account.type";
import { paymentApi } from "../services/payment.api";
import { productApi } from "../services/product.api";
import { accountApi } from "../services/account.api";
import { message, Spin } from "antd";
import { LoadingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";

interface ChartData {
    name: string;
    successful: number;
    failed: number;
    total: number;
}

interface BestSeller {
    productPay: string;
    count: number;
}

const antIcon = <LoadingOutlined style={{ fontSize: 40 }} spin />;

const DashboardPage = () => {
    const [loading, setLoading] = useState(true);
    const [payments, setPayments] = useState<Payment[]>([]);
    const [products, setProducts] = useState<Product[]>([]);
    const [accounts, setAccounts] = useState<Account[]>([]);
    const [bestSellers, setBestSellers] = useState<BestSeller[]>([]);
    const [salesData, setSalesData] = useState<ChartData[]>([]);

    const fetchAllData = async () => {
        try {
            setLoading(true);
            const [paymentResponse, productResponse, accountResponse] =
                await Promise.all([
                    paymentApi.getPayments({
                        page: 1,
                        pageSize: 1000,
                    }),
                    productApi.getProducts({
                        page: 1,
                        pageSize: 1000,
                    }),
                    accountApi.getAccounts({
                        page: 1,
                        pageSize: 1000,
                    }),
                ]);

            setPayments(paymentResponse.payments);
            setProducts(productResponse.products);
            setAccounts(accountResponse.accounts);

            // Process sales data
            const monthlyData = paymentResponse.payments.reduce<
                Record<string, { successful: number; failed: number }>
            >((acc, payment) => {
                const month = dayjs(payment.paymentDate).format("MMM YYYY");
                if (!acc[month]) {
                    acc[month] = { successful: 0, failed: 0 };
                }
                if (payment.paymentStatus) {
                    acc[month].successful += 1;
                } else {
                    acc[month].failed += 1;
                }
                return acc;
            }, {});

            const chartData = Object.entries(monthlyData)
                .map(([month, stats]) => ({
                    name: month,
                    successful: stats.successful,
                    failed: stats.failed,
                    total: stats.successful + stats.failed,
                }))
                .sort(
                    (a, b) =>
                        dayjs(a.name, "MMM YYYY").valueOf() -
                        dayjs(b.name, "MMM YYYY").valueOf()
                );

            setSalesData(chartData);

            // Calculate best selling products
            const productCounts = paymentResponse.payments
                .filter((p) => p.paymentStatus)
                .reduce<Record<string, number>>((acc, payment) => {
                    acc[payment.productPay] =
                        (acc[payment.productPay] || 0) + 1;
                    return acc;
                }, {});

            const bestSellersData = Object.entries(productCounts)
                .map(([productPay, count]) => ({ productPay, count }))
                .sort((a, b) => b.count - a.count)
                .slice(0, 5);

            setBestSellers(bestSellersData);
        } catch (error) {
            console.error("Error fetching dashboard data:", error);
            message.error("Failed to fetch dashboard data");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAllData();
    }, []);

    return (
        <BaseLayout>
            <Spin
                spinning={loading}
                indicator={antIcon}
                tip="Loading dashboard data..."
                size="large"
                style={{
                    maxHeight: "100vh",
                    minHeight: "100vh",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                }}
            >
                <DashboardSection
                    loading={loading}
                    accounts={accounts}
                    products={products}
                    payments={payments}
                    salesData={salesData}
                    bestSellers={bestSellers}
                />
            </Spin>
        </BaseLayout>
    );
};

export default DashboardPage;
