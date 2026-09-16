export interface PaginationState {
    currentPage: number;
    pageSize: number;
    totalPages: number;
}

export interface PaginationParams {
    page: number;
    pageSize: number;
}

export interface BasePaginatedData<T> {
    items: T[];
    totalItems: number;
}

export interface ProductPaginatedResponse
    extends PaginationState,
        BasePaginatedData<import("./product.type").Product> {
    products: import("./product.type").Product[];
    totalProducts: number;
}

export interface PaymentPaginatedResponse
    extends PaginationState,
        BasePaginatedData<import("./payment.type").Payment> {
    payments: import("./payment.type").Payment[];
    totalPayments: number;
}
