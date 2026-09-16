import React from "react";
import { Modal, Descriptions, Carousel } from "antd";
import { ProductDetail } from "../../../types/product.type";
import dayjs from "dayjs";

interface ProductDetailModalProps {
    open: boolean;
    onCancel: () => void;
    product: ProductDetail;
}

const ProductDetailModal: React.FC<ProductDetailModalProps> = ({
    open,
    onCancel,
    product,
}) => {
    return (
        <Modal
            title="Product Details"
            open={open}
            onCancel={onCancel}
            footer={null}
            width={800}
        >
            <div style={{ marginBottom: 24 }}>
                <Carousel autoplay>
                    {product.imageUrls.map((url, index) => (
                        <div key={index}>
                            <img
                                src={url}
                                alt={`Product ${index + 1}`}
                                style={{
                                    width: "100%",
                                    height: "400px",
                                    objectFit: "cover",
                                }}
                            />
                        </div>
                    ))}
                </Carousel>
            </div>

            <Descriptions bordered column={1}>
                <Descriptions.Item label="Title">
                    {product.title}
                </Descriptions.Item>
                <Descriptions.Item label="Price">
                    ₫{product.price.toLocaleString("vi-VN")}
                </Descriptions.Item>
                <Descriptions.Item label="Tag">{product.tag}</Descriptions.Item>
                <Descriptions.Item label="Features">
                    {product.feature}
                </Descriptions.Item>
                <Descriptions.Item label="Technologies Used">
                    {product.technologyUsed}
                </Descriptions.Item>
                <Descriptions.Item label="Posted Date">
                    {dayjs(product.postedDate).format("DD/MM/YYYY HH:mm")}
                </Descriptions.Item>
                <Descriptions.Item label="Detailed Description">
                    <div
                        dangerouslySetInnerHTML={{
                            __html: product.descriptionDetail,
                        }}
                    />
                </Descriptions.Item>
            </Descriptions>
        </Modal>
    );
};

export default ProductDetailModal;
