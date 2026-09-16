import { Form, Input, Button, Checkbox } from "antd";
import PropTypes from "prop-types";
import { useState } from "react";

const GenericForm = ({ fields, onFinish, onFinishFailed, checkboxContent }) => {
    const [loading, setLoading] = useState(false);

    const handleFinish = async (values) => {
        setLoading(true);
        try {
            await onFinish(values);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Form
            name="genericForm"
            initialValues={{ remember: true }}
            onFinish={handleFinish}
            onFinishFailed={onFinishFailed}
            layout="vertical"
        >
            {fields.map((field, index) => (
                <Form.Item
                    key={index}
                    label={field.label}
                    name={field.name}
                    rules={[
                        {
                            required: field.required,
                            message: field.message,
                        },
                    ]}
                >
                    {field.type === "password" ? <Input.Password /> : <Input />}
                </Form.Item>
            ))}
            {checkboxContent && (
                <Form.Item name="remember" valuePropName="checked">
                    <Checkbox>{checkboxContent}</Checkbox>
                </Form.Item>
            )}

            <Form.Item>
                <Button
                    type="primary"
                    htmlType="submit"
                    className="w-full"
                    loading={loading}
                >
                    Submit
                </Button>
            </Form.Item>
        </Form>
    );
};

GenericForm.propTypes = {
    fields: PropTypes.arrayOf(
        PropTypes.shape({
            label: PropTypes.string.isRequired,
            name: PropTypes.string.isRequired,
            type: PropTypes.string,
            required: PropTypes.bool,
            message: PropTypes.string,
        })
    ).isRequired,
    onFinish: PropTypes.func.isRequired,
    onFinishFailed: PropTypes.func.isRequired,
    checkboxContent: PropTypes.string,
};

export default GenericForm;
