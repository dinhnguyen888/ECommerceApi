import PropTypes from "prop-types";
import { Avatar, List, Button } from "antd";
import { Comment } from "@ant-design/compatible";
import { UserOutlined, MessageOutlined } from "@ant-design/icons";

const CommentList = ({ comments, onReply }) => (
    <List
        dataSource={comments}
        itemLayout="horizontal"
        renderItem={(comment) => (
            <Comment
                author={comment.userName}
                avatar={<Avatar icon={<UserOutlined />} />}
                content={comment.content}
                datetime={new Date(comment.createdAt).toLocaleString()}
                actions={[
                    <Button
                        key="reply"
                        type="link"
                        icon={<MessageOutlined />}
                        onClick={() => onReply(comment)}
                    >
                        Trả lời
                    </Button>,
                ]}
            >
                {comment.replies?.map((reply) => (
                    <Comment
                        key={reply.id}
                        author={reply.userName}
                        avatar={<Avatar icon={<UserOutlined />} />}
                        content={reply.content}
                        datetime={new Date(reply.createdAt).toLocaleString()}
                    />
                ))}
            </Comment>
        )}
    />
);

CommentList.propTypes = {
    comments: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.string.isRequired,
            userName: PropTypes.string.isRequired,
            content: PropTypes.string.isRequired,
            createdAt: PropTypes.string.isRequired,
            replies: PropTypes.arrayOf(
                PropTypes.shape({
                    id: PropTypes.string.isRequired,
                    userName: PropTypes.string.isRequired,
                    content: PropTypes.string.isRequired,
                    createdAt: PropTypes.string.isRequired,
                })
            ),
        })
    ).isRequired,
    onReply: PropTypes.func.isRequired,
};

export default CommentList;
