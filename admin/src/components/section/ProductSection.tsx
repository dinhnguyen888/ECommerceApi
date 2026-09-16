import { Card, Tag, Button, Tooltip } from "antd";
import {
    CreateProductRequest,
    Product,
    ProductDetail,
    ProductResponse,
    UpdateProductRequest,
} from "../../types/product.type";
import BaseTable from "../common/BaseTable";
import { ColumnsType } from "antd/es/table";
import dayjs from "dayjs";
import {
    EditOutlined,
    PlusOutlined,
    DeleteOutlined,
    EyeOutlined,
    CommentOutlined,
} from "@ant-design/icons";
import ProductAddModal from "../modal/product/ProductAddModal";
import ProductEditModal from "../modal/product/ProductEditModal";
import ProductDetailModal from "../modal/product/ProductDetailModal";

interface ProductSectionProps {
    data: ProductResponse;
    loading: boolean;
    onPageChange: (page: number, pageSize: number) => void;
    refreshData: () => void;
    modalLoading: boolean;
    addModalOpen: boolean;
    editModalOpen: boolean;
    detailModalOpen: boolean;
    selectedProduct: UpdateProductRequest | null;
    productDetail: ProductDetail | null;
    onAddModalOpen: () => void;
    onAddModalClose: () => void;
    onEditModalClose: () => void;
    onDetailModalClose: () => void;
    onAdd: (values: CreateProductRequest) => Promise<void>;
    onEdit: (values: UpdateProductRequest) => Promise<void>;
    onDelete: (id: string) => void;
    onView: (id: string) => void;
    onEditClick: (id: string) => void;
    onViewComments: (id: string) => void;
}

const ProductSection = ({
    data,
    loading,
    onPageChange,
    modalLoading,
    addModalOpen,
    editModalOpen,
    detailModalOpen,
    selectedProduct,
    productDetail,
    onAddModalOpen,
    onAddModalClose,
    onEditModalClose,
    onDetailModalClose,
    onAdd,
    onEdit,
    onDelete,
    onView,
    onEditClick,
    onViewComments,
}: ProductSectionProps) => {
    const columns: ColumnsType<Product> = [
        {
            title: "Image",
            dataIndex: "imageUrls",
            key: "imageUrls",
            width: 100,
            render: (imageUrls: string[]) => (
                <img
                    src={
                        imageUrls && imageUrls.length > 0
                            ? imageUrls[0]
                            : "https://placehold.co/100x60"
                    }
                    alt="product"
                    style={{
                        width: "80px",
                        height: "48px",
                        objectFit: "cover",
                        borderRadius: "4px",
                    }}
                />
            ),
        },
        {
            title: "Title",
            dataIndex: "title",
            key: "title",
            width: 180,
            ellipsis: {
                showTitle: false,
            },
            render: (text: string) => (
                <Tooltip placement="topLeft" title={text}>
                    <strong>{text}</strong>
                </Tooltip>
            ),
        },
        {
            title: "Price",
            dataIndex: "price",
            key: "price",
            width: 100,
            render: (price: number) => (
                <span>₫{price.toLocaleString("vi-VN")}</span>
            ),
            sorter: (a, b) => a.price - b.price,
        },
        {
            title: "Tag",
            dataIndex: "tag",
            key: "tag",
            width: 90,
            render: (tag: string) => <Tag color="blue">{tag}</Tag>,
            filters: [
                { text: "Tool", value: "tool" },
                { text: "Web App", value: "webapp" },
                { text: "Software", value: "phanmem" },
            ],
            onFilter: (value, record) => record.tag === value,
        },
        {
            title: "Posted Date",
            dataIndex: "postedDate",
            key: "postedDate",
            width: 120,
            render: (date: string) => dayjs(date).format("DD/MM/YYYY HH:mm"),
            sorter: (a, b) =>
                dayjs(a.postedDate).unix() - dayjs(b.postedDate).unix(),
        },
        {
            title: "Description",
            dataIndex: "description",
            key: "description",
            width: 300,
            ellipsis: {
                showTitle: false,
            },
            render: (text: string) => (
                <Tooltip placement="topLeft" title={text}>
                    {text}
                </Tooltip>
            ),
        },
        {
            title: "Actions",
            key: "actions",
            width: 130,
            fixed: "right",
            render: (_, record) => (
                <Button.Group>
                    <Tooltip title="View Details">
                        <Button
                            icon={<EyeOutlined />}
                            onClick={() => onView(record.id)}
                            type="text"
                        />
                    </Tooltip>
                    <Tooltip title="View Comments">
                        <Button
                            icon={<CommentOutlined />}
                            onClick={() => onViewComments(record.id)}
                            type="text"
                        />
                    </Tooltip>
                    <Tooltip title="Edit">
                        <Button
                            icon={<EditOutlined />}
                            onClick={() => onEditClick(record.id)}
                            type="text"
                        />
                    </Tooltip>
                    <Tooltip title="Delete">
                        <Button
                            icon={<DeleteOutlined />}
                            onClick={() => onDelete(record.id)}
                            type="text"
                            danger
                        />
                    </Tooltip>
                </Button.Group>
            ),
        },
    ];

    return (
        <Card
            title="Products"
            extra={
                <Button
                    type="primary"
                    icon={<PlusOutlined />}
                    onClick={onAddModalOpen}
                >
                    Add Product
                </Button>
            }
        >
            <BaseTable<Product, ProductResponse>
                columns={columns}
                data={data}
                loading={loading}
                onPageChange={onPageChange}
                scroll={{ x: 1020 }}
                dataSource={data.products}
            />

            <ProductAddModal
                open={addModalOpen}
                onCancel={onAddModalClose}
                onSubmit={onAdd}
                loading={modalLoading}
            />

            {selectedProduct && (
                <ProductEditModal
                    open={editModalOpen}
                    onCancel={onEditModalClose}
                    onSubmit={onEdit}
                    loading={modalLoading}
                    product={selectedProduct}
                />
            )}

            {productDetail && (
                <ProductDetailModal
                    open={detailModalOpen}
                    onCancel={onDetailModalClose}
                    product={productDetail}
                />
            )}
        </Card>
    );
};

export default ProductSection;
