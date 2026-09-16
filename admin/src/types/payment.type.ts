import type { PaginationParams, PaymentListResponse } from "./base.type";

export type PaymentGateway = "VNPAY" | "PAYPAL" | "MOMO";

export interface Payment {
    id: number;
    productPay: string;
    productId: string;
    userId: string;
    paymentGateway: PaymentGateway;
    productPrice: number;
    paymentDate: string;
    paymentStatus: boolean;
}

export type { PaginationParams };
export type { PaymentListResponse as PaymentResponse };
