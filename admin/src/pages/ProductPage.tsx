import { useState, useEffect } from "react";
import { message } from "antd";
import { useNavigate } from "react-router-dom";
import BaseLayout from "../components/layout/BaseLayout";
import ProductSection from "../components/section/ProductSection";
import {
    PaginationParams,
    ProductResponse,
    CreateProductRequest,
    UpdateProductRequest,
    ProductDetail,
} from "../types/product.type";
import { productApi } from "../services/product.api";

const initialPagination: PaginationParams = {
    page: 1,
    pageSize: 6,
};

export const ProductPage = () => {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [modalLoading, setModalLoading] = useState(false);
    const [addModalOpen, setAddModalOpen] = useState(false);
    const [editModalOpen, setEditModalOpen] = useState(false);
    const [detailModalOpen, setDetailModalOpen] = useState(false);
    const [selectedProduct, setSelectedProduct] =
        useState<UpdateProductRequest | null>(null);
    const [productDetail, setProductDetail] = useState<ProductDetail | null>(
        null
    );
    const [data, setData] = useState<ProductResponse>({
        currentPage: 1,
        pageSize: 6,
        totalProducts: 0,
        totalPages: 0,
        products: [],
    });

    const fetchProducts = async (params: PaginationParams) => {
        try {
            setLoading(true);
            const response = await productApi.getProducts(params);
            setData(response);
        } catch (error) {
            console.error("Error fetching products:", error);
            message.error("Không thể tải danh sách sản phẩm!");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchProducts(initialPagination);
    }, []);

    const handlePageChange = (page: number, pageSize: number) => {
        fetchProducts({ page, pageSize });
    };

    const handleAdd = async (values: CreateProductRequest) => {
        setModalLoading(true);
        try {
            await productApi.createProduct(values);
            message.success("Product added successfully!");
            setAddModalOpen(false);
            fetchProducts({
                page: data.currentPage,
                pageSize: data.pageSize,
            });
        } catch (error) {
            console.error("Add product error:", error);
            message.error("Failed to add product!");
        } finally {
            setModalLoading(false);
        }
    };

    const handleEdit = async (values: UpdateProductRequest) => {
        if (!selectedProduct?.id) return;
        setModalLoading(true);
        try {
            await productApi.updateProduct(selectedProduct.id, {
                ...selectedProduct, // Keep all existing fields
                ...values, // Override with new values
                id: selectedProduct.id, // Ensure ID is correct
            });
            message.success("Product updated successfully!");
            setEditModalOpen(false);
            setSelectedProduct(null);
            fetchProducts({
                page: data.currentPage,
                pageSize: data.pageSize,
            });
        } catch (error) {
            console.error("Update product error:", error);
            message.error("Failed to update product!");
        } finally {
            setModalLoading(false);
        }
    };

    const handleDelete = async (id: string) => {
        try {
            await productApi.deleteProduct(id);
            message.success("Product deleted successfully!");
            fetchProducts({
                page: data.currentPage,
                pageSize: data.pageSize,
            });
        } catch (error) {
            console.error("Delete product error:", error);
            message.error("Failed to delete product!");
        }
    };

    const handleView = async (id: string) => {
        try {
            const detail = await productApi.getProductDetail(id);
            setProductDetail(detail);
            setDetailModalOpen(true);
        } catch (error) {
            console.error("Get product detail error:", error);
            message.error("Failed to get product details!");
        }
    };

    const handleEditClick = async (id: string) => {
        try {
            const productData = await productApi.getProductForUpdate(id);
            setSelectedProduct(productData);
            setEditModalOpen(true);
        } catch (error) {
            console.error("Get product for update error:", error);
            message.error("Failed to get product data for editing!");
        }
    };

    const handleViewComments = (id: string) => {
        navigate(`/products/${id}/comments`);
    };

    return (
        <BaseLayout>
            <ProductSection
                data={data}
                loading={loading}
                modalLoading={modalLoading}
                addModalOpen={addModalOpen}
                editModalOpen={editModalOpen}
                detailModalOpen={detailModalOpen}
                selectedProduct={selectedProduct}
                productDetail={productDetail}
                onPageChange={handlePageChange}
                onAddModalOpen={() => setAddModalOpen(true)}
                onAddModalClose={() => setAddModalOpen(false)}
                onEditModalClose={() => {
                    setEditModalOpen(false);
                    setSelectedProduct(null);
                }}
                onDetailModalClose={() => {
                    setDetailModalOpen(false);
                    setProductDetail(null);
                }}
                onAdd={handleAdd}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onView={handleView}
                onEditClick={handleEditClick}
                onViewComments={handleViewComments}
                refreshData={() =>
                    fetchProducts({
                        page: data.currentPage,
                        pageSize: data.pageSize,
                    })
                }
            />
        </BaseLayout>
    );
};

export default ProductPage;
