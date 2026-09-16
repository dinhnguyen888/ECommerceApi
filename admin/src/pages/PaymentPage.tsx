import { useState, useEffect } from "react";
import BaseLayout from "../components/layout/BaseLayout";
import PaymentSection from "../components/section/PaymentSection";
import { PaginationParams, PaymentResponse } from "../types/payment.type";
import { message } from "antd";
import { paymentApi } from "../services/payment.api";

const initialPagination: PaginationParams = {
    page: 1,
    pageSize: 10,
};

export const PaymentPage = () => {
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<PaymentResponse>({
        currentPage: 1,
        pageSize: 10,
        totalPayments: 0,
        totalPages: 0,
        payments: [],
    });

    const fetchPayments = async (params: PaginationParams) => {
        try {
            setLoading(true);
            const response = await paymentApi.getPayments(params);
            setData(response);
        } catch (error) {
            console.error("Error fetching payments:", error);
            message.error("Failed to fetch payments");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPayments(initialPagination);
    }, []);

    const handlePageChange = (page: number, pageSize: number) => {
        fetchPayments({ page, pageSize });
    };

    const handleDeleteOk = async () => {
        try {
            await paymentApi.deletePendingPayment();
            message.success("Pending payments deleted successfully");
            await fetchPayments({
                page: data.currentPage,
                pageSize: data.pageSize,
            });
        } catch (error) {
            console.error("Error deleting pending payments:", error);
            message.error(
                "Failed to delete pending payments. Please try again."
            );
        }
    };

    return (
        <BaseLayout>
            <PaymentSection
                data={data}
                loading={loading}
                onPageChange={handlePageChange}
                onDeleteOk={handleDeleteOk}
            />
        </BaseLayout>
    );
};

export default PaymentPage;
