import { Button, Form, Input, Modal, message } from "antd";
import { useState } from "react";
import { addBanner } from "../../../services/banner.api";
import UploadWidget from "../../common/UploadWidget";
import { BannerAddRequest } from "../../../types/banner.type";

interface BannerAddModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
}

export const BannerAddModal = ({
    isOpen,
    onClose,
    onSuccess,
}: BannerAddModalProps) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [imageUrl, setImageUrl] = useState("");

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            setLoading(true);

            const bannerData: BannerAddRequest = {
                bannerName: values.bannerName,
                bannerUrl: imageUrl,
            };

            await addBanner(bannerData);
            message.success("Banner added successfully");
            form.resetFields();
            setImageUrl("");
            onSuccess();
            onClose();
        } catch (error: Error | unknown) {
            message.error(
                error instanceof Error ? error.message : "Failed to add banner"
            );
        } finally {
            setLoading(false);
        }
    };

    const handleImageUpload = (url: string) => {
        setImageUrl(url);
    };

    return (
        <Modal
            title="Add New Banner"
            open={isOpen}
            onCancel={onClose}
            footer={[
                <Button key="cancel" onClick={onClose}>
                    Cancel
                </Button>,
                <Button
                    key="submit"
                    type="primary"
                    loading={loading}
                    onClick={handleSubmit}
                    disabled={!imageUrl}
                >
                    Add Banner
                </Button>,
            ]}
        >
            <Form form={form} layout="vertical">
                <Form.Item
                    name="bannerName"
                    label="Banner Name"
                    rules={[
                        { required: true, message: "Please enter banner name" },
                    ]}
                >
                    <Input placeholder="Enter banner name" />
                </Form.Item>

                <Form.Item label="Banner Image" required>
                    <UploadWidget onUpload={handleImageUpload} />
                    {imageUrl && (
                        <img
                            src={imageUrl}
                            alt="Banner preview"
                            style={{
                                marginTop: 16,
                                width: "100%",
                                maxHeight: 200,
                                objectFit: "contain",
                            }}
                        />
                    )}
                </Form.Item>
            </Form>
        </Modal>
    );
};
