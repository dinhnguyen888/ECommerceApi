import PropTypes from "prop-types";
import { Form, Button, Input, Avatar } from "antd";
import { UserOutlined } from "@ant-design/icons";
import { Comment } from "@ant-design/compatible";

const { TextArea } = Input;

const CommentEditor = ({
    onSubmit,
    submitting,
    value,
    onChange,
    placeholder = "Viết bình luận của bạn...",
    submitText = "Gửi",
    showAvatar = true,
}) => {
    const editor = (
        <div className="mt-4">
            <Form.Item>
                <TextArea
                    rows={4}
                    onChange={onChange}
                    value={value}
                    placeholder={placeholder}
                />
            </Form.Item>
            <Form.Item>
                <Button
                    htmlType="submit"
                    loading={submitting}
                    onClick={onSubmit}
                    type="primary"
                >
                    {submitText}
                </Button>
            </Form.Item>
        </div>
    );

    if (!showAvatar) return editor;

    return (
        <Comment avatar={<Avatar icon={<UserOutlined />} />} content={editor} />
    );
};

CommentEditor.propTypes = {
    onSubmit: PropTypes.func.isRequired,
    submitting: PropTypes.bool,
    value: PropTypes.string,
    onChange: PropTypes.func.isRequired,
    placeholder: PropTypes.string,
    submitText: PropTypes.string,
    showAvatar: PropTypes.bool,
};

export default CommentEditor;
