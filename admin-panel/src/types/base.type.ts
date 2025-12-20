export interface PaginationParams {
    page: number;
    pageSize: number;
}

export interface PaginationMeta {
    currentPage: number;
    pageSize: number;
    totalPages: number;
}

export interface PaginatedData<T> {
    items: T[];
    total: number;
}

// Specific response types
export interface ProductListResponse extends PaginationMeta {
    products: import("./product.type").Product[];
    totalProducts: number;
}

export interface PaymentListResponse extends PaginationMeta {
    payments: import("./payment.type").Payment[];
    totalPayments: number;
}
