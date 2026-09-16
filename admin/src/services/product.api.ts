import {
    CreateProductRequest,
    PaginationParams,
    ProductDetail,
    ProductResponse,
    UpdateProductRequest,
} from "../types/product.type";
import { axiosInstance } from "./apiConfig";

export const productApi = {
    getProducts: async (params: PaginationParams): Promise<ProductResponse> => {
        const response = await axiosInstance.get("/Products", {
            params: {
                page: params.page,
                pageSize: params.pageSize,
            },
        });
        return response.data;
    },

    getProductDetail: async (id: string): Promise<ProductDetail> => {
        const response = await axiosInstance.get(`/Products/detail/${id}`);
        return response.data;
    },

    getProductForUpdate: async (id: string): Promise<UpdateProductRequest> => {
        const response = await axiosInstance.get(
            `/Products/for-update-product/${id}`
        );
        return response.data;
    },

    createProduct: async (data: CreateProductRequest): Promise<void> => {
        await axiosInstance.post("/Products", data);
    },

    updateProduct: async (
        id: string,
        data: UpdateProductRequest
    ): Promise<void> => {
        await axiosInstance.put(`/Products/${id}`, data);
    },

    deleteProduct: async (id: string): Promise<void> => {
        await axiosInstance.delete(`/Products/${id}`);
    },
};

export default productApi;
