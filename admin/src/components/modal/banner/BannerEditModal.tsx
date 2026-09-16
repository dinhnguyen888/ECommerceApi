import { Button, Form, Input, Modal, message } from "antd";
import { useEffect, useState } from "react";
import { updateBanner } from "../../../services/banner.api";
import UploadWidget from "../../common/UploadWidget";
import { Banner, BannerAddRequest } from "../../../types/banner.type";

interface BannerEditModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    banner: Banner;
}

export const BannerEditModal = ({
    isOpen,
    onClose,
    onSuccess,
    banner,
}: BannerEditModalProps) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [imageUrl, setImageUrl] = useState("");

    useEffect(() => {
        if (banner) {
            form.setFieldsValue({
                bannerName: banner.bannerName,
            });
            setImageUrl(banner.bannerUrl);
        }
    }, [banner, form]);

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            setLoading(true);

            const bannerData: BannerAddRequest = {
                bannerName: values.bannerName,
                bannerUrl: imageUrl,
            };

            await updateBanner(banner.id, bannerData);
            message.success("Banner updated successfully");
            onSuccess();
            onClose();
        } catch (error: Error | unknown) {
            message.error(
                error instanceof Error
                    ? error.message
                    : "Failed to update banner"
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
            title="Edit Banner"
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
                    Update Banner
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
