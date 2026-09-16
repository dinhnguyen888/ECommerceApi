import apiClient from "./APIConfig";

export const CreatePayment = async (payment, paymentGateway) => {
    try {
        const queryParams = {
            ProductPay: payment.productName,
            ProductId: payment.productId,
            UserId: payment.userId,
            PaymentGateway: paymentGateway,
            ProductPrice: payment.price,
        };
        console.log(queryParams);

        switch (paymentGateway) {
            case "PAYOS":
                return await apiClient.post("/Payos/create", null, {
                    params: queryParams,
                });
            case "PAYPAL":
                return await apiClient.post(
                    "/PayPal/create-paypal-payment",
                    null,
                    {
                        params: queryParams,
                    }
                );
            case "VNPAY":
                return await apiClient.get("/Vnpay/CreatePaymentUrl", {
                    params: queryParams,
                });
            case "MOMO":
                return await apiClient.get("/Momo/create-payment", {
                    params: queryParams,
                });
            default:
                throw new Error("Invalid payment gateway");
        }
    } catch (error) {
        throw new Error(error.response?.data || "Failed to create payment");
    }
};
