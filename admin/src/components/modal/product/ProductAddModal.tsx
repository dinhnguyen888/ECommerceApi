import React, { useState } from "react";
import { Modal, Form, Input, InputNumber, Select, Button, message } from "antd";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import { CreateProductRequest } from "../../../types/product.type";
import UploadWidget from "../../common/UploadWidget";

interface ProductAddModalProps {
    open: boolean;
    onCancel: () => void;
    onSubmit: (values: CreateProductRequest) => Promise<void>;
    loading?: boolean;
}

const imageGridStyles: React.CSSProperties = {
    display: "flex",
    gap: "8px",
    flexWrap: "wrap",
    marginTop: "8px",
};

const imageStyles: React.CSSProperties = {
    width: "100px",
    height: "100px",
    objectFit: "cover",
    borderRadius: "4px",
};

const ProductAddModal: React.FC<ProductAddModalProps> = ({
    open,
    onCancel,
    onSubmit,
    loading,
}) => {
    const [form] = Form.useForm();
    const [imageUrls, setImageUrls] = useState<string[]>([]);

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            await onSubmit({
                ...values,
                imageUrls: imageUrls,
            });
            form.resetFields();
            setImageUrls([]);
        } catch (error) {
            console.error("Validation failed:", error);
        }
    };

    const handleImageUpload = (url: string) => {
        setImageUrls((prev) => [...prev, url]);
        message.success("Image uploaded successfully!");
    };

    return (
        <Modal
            title="Add New Product"
            open={open}
            onCancel={onCancel}
            footer={[
                <Button key="back" onClick={onCancel}>
                    Cancel
                </Button>,
                <Button
                    key="submit"
                    type="primary"
                    loading={loading}
                    onClick={handleSubmit}
                >
                    Add
                </Button>,
            ]}
            width={800}
        >
            <Form
                form={form}
                layout="vertical"
                initialValues={{
                    tag: "tool",
                }}
            >
                <div
                    style={{
                        display: "grid",
                        gridTemplateColumns: "1fr 1fr",
                        gap: "16px",
                    }}
                >
                    <Form.Item
                        name="title"
                        label="Title"
                        rules={[
                            { required: true, message: "Please enter title!" },
                        ]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        name="price"
                        label="Price"
                        rules={[
                            { required: true, message: "Please enter price!" },
                        ]}
                    >
                        <InputNumber
                            style={{ width: "100%" }}
                            formatter={(value) =>
                                `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                            }
                            parser={(value) =>
                                value!.replace(/\$\s?|(,*)/g, "")
                            }
                        />
                    </Form.Item>
                </div>

                <div
                    style={{
                        display: "grid",
                        gridTemplateColumns: "2fr 1fr",
                        gap: "16px",
                    }}
                >
                    <Form.Item
                        name="productUrl"
                        label="Product URL"
                        rules={[
                            {
                                required: true,
                                message: "Please enter product URL!",
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        name="tag"
                        label="Tag"
                        rules={[
                            { required: true, message: "Please select a tag!" },
                        ]}
                    >
                        <Select>
                            <Select.Option value="tool">Tool</Select.Option>
                            <Select.Option value="webapp">
                                Web App
                            </Select.Option>
                            <Select.Option value="phanmem">
                                Software
                            </Select.Option>
                        </Select>
                    </Form.Item>
                </div>

                <Form.Item
                    name="description"
                    label="Short Description"
                    rules={[
                        {
                            required: true,
                            message: "Please enter description!",
                        },
                    ]}
                >
                    <Input.TextArea rows={2} />
                </Form.Item>

                <Form.Item
                    name="feature"
                    label="Features"
                    rules={[
                        { required: true, message: "Please enter features!" },
                    ]}
                >
                    <Input.TextArea rows={3} />
                </Form.Item>

                <Form.Item
                    name="technologyUsed"
                    label="Technologies Used"
                    rules={[
                        {
                            required: true,
                            message: "Please enter technologies used!",
                        },
                    ]}
                >
                    <Input.TextArea rows={2} />
                </Form.Item>

                <Form.Item
                    name="descriptionDetail"
                    label="Detailed Description"
                    rules={[
                        {
                            required: true,
                            message: "Please enter detailed description!",
                        },
                    ]}
                >
                    <ReactQuill
                        theme="snow"
                        style={{ height: "300px", marginBottom: "40px" }}
                    />
                </Form.Item>

                <Form.Item label="Images">
                    <div style={{ marginBottom: 16 }}>
                        <UploadWidget onUpload={handleImageUpload} />
                    </div>
                    {imageUrls.length > 0 && (
                        <div style={imageGridStyles}>
                            {imageUrls.map((url, index) => (
                                <img
                                    key={index}
                                    src={url}
                                    alt={`Product ${index + 1}`}
                                    style={imageStyles}
                                />
                            ))}
                        </div>
                    )}
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default ProductAddModal;
