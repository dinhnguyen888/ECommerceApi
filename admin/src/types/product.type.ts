import type { PaginationParams, ProductListResponse } from "./base.type";

export interface Product {
    id: string;
    title: string;
    price: number;
    description: string;
    tag: string;
    imageUrls: string[];
    postedDate: string;
}

export interface CreateProductRequest {
    title: string;
    description: string;
    descriptionDetail: string;
    tag: string;
    price: number;
    feature: string;
    technologyUsed: string;
    imageUrls: string[];
    productUrl: string;
}

export interface UpdateProductRequest {
    id: string;
    title: string;
    description: string;
    descriptionDetail: string;
    tag: string;
    price: number;
    feature: string;
    technologyUsed: string;
    imageUrls: string[];
    productUrl: string;
    postedDate: string;
}

export interface ProductDetail {
    id: string;
    title: string;
    price: number;
    feature: string;
    technologyUsed: string;
    tag: string;
    imageUrls: string[];
    descriptionDetail: string;
    postedDate: string;
}

export type { PaginationParams };
export type { ProductListResponse as ProductResponse };
