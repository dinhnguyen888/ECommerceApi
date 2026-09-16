import { Form, Input, Modal } from "antd";
import { News } from "../../../types/news.type";
import { useEffect } from "react";

interface NewsEditModalProps {
    open: boolean;
    onCancel: () => void;
    onSubmit: (values: News) => Promise<void>;
    loading: boolean;
    news: News;
}

const NewsEditModal = ({
    open,
    onCancel,
    onSubmit,
    loading,
    news,
}: NewsEditModalProps) => {
    const [form] = Form.useForm();

    useEffect(() => {
        if (open && news) {
            form.setFieldsValue(news);
        }
    }, [open, news, form]);

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            await onSubmit({ ...news, ...values });
            form.resetFields();
        } catch (error) {
            console.error("Validation failed:", error);
        }
    };

    return (
        <Modal
            title="Edit News"
            open={open}
            onCancel={onCancel}
            onOk={handleSubmit}
            confirmLoading={loading}
            destroyOnClose
        >
            <Form form={form} layout="vertical" initialValues={news}>
                <Form.Item
                    name="title"
                    label="Title"
                    rules={[
                        { required: true, message: "Please input the title!" },
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    name="linkDetail"
                    label="Link Detail"
                    rules={[
                        { required: true, message: "Please input the link!" },
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    name="imageUrl"
                    label="Image URL"
                    rules={[
                        {
                            required: true,
                            message: "Please input the image URL!",
                        },
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    name="type"
                    label="Type"
                    rules={[
                        { required: true, message: "Please input the type!" },
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    name="description"
                    label="Description"
                    rules={[
                        {
                            required: true,
                            message: "Please input the description!",
                        },
                    ]}
                >
                    <Input.TextArea rows={4} />
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default NewsEditModal;
