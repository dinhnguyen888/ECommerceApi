import { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { message } from "antd";
import CommentList from "./common/CommentList";
import CommentEditor from "./common/CommentEditor";
import {
    getCommentsByPageId,
    postComment,
    replyComment,
} from "../services/comment.service";

const ProductComment = ({ pageId, currentUser }) => {
    const [comments, setComments] = useState([]);
    const [submitting, setSubmitting] = useState(false);
    const [commentValue, setCommentValue] = useState("");
    const [replyingTo, setReplyingTo] = useState(null);

    useEffect(() => {
        fetchComments();
    }, [pageId]);

    const fetchComments = async () => {
        try {
            const data = await getCommentsByPageId(pageId);
            setComments(data);
        } catch (error) {
            message.error("Không thể tải bình luận: " + error.message);
        }
    };

    const handleSubmitComment = async () => {
        if (!commentValue) {
            message.warning("Vui lòng nhập nội dung bình luận!");
            return;
        }

        setSubmitting(true);
        try {
            if (replyingTo) {
                const replyData = {
                    pageId,
                    commentId: replyingTo.id,
                    userId: currentUser.id,
                    userName: currentUser.name,
                    content: commentValue,
                };
                await replyComment(replyData);
                message.success("Đã trả lời bình luận thành công!");
            } else {
                const commentData = {
                    pageId,
                    userId: currentUser.id,
                    userName: currentUser.name,
                    content: commentValue,
                };
                await postComment(commentData);
                message.success("Đã đăng bình luận thành công!");
            }

            setCommentValue("");
            setReplyingTo(null);
            await fetchComments();
        } catch (error) {
            message.error(
                replyingTo
                    ? "Không thể trả lời bình luận: " + error.message
                    : "Không thể đăng bình luận: " + error.message
            );
        } finally {
            setSubmitting(false);
        }
    };

    const handleReply = (comment) => {
        setReplyingTo(comment);
        setCommentValue("");
    };

    return (
        <div className="mt-8">
            <h2 className="text-2xl font-semibold mb-4">Bình luận</h2>

            {replyingTo && (
                <div className="bg-gray-50 p-4 rounded-lg mb-4">
                    <div className="flex justify-between items-center mb-2">
                        <h3 className="text-sm text-gray-600">
                            Đang trả lời bình luận của {replyingTo.userName}
                        </h3>
                        <button
                            onClick={() => setReplyingTo(null)}
                            className="text-gray-500 hover:text-gray-700"
                        >
                            ✕
                        </button>
                    </div>
                    <p className="text-gray-600 italic mb-2">
                        &ldquo;{replyingTo.content}&rdquo;
                    </p>
                </div>
            )}

            <CommentEditor
                value={commentValue}
                onChange={(e) => setCommentValue(e.target.value)}
                onSubmit={handleSubmitComment}
                submitting={submitting}
                placeholder={
                    replyingTo
                        ? "Viết câu trả lời của bạn..."
                        : "Viết bình luận của bạn..."
                }
                submitText={replyingTo ? "Trả lời" : "Gửi bình luận"}
            />

            {comments.length > 0 && (
                <div className="mt-8">
                    <CommentList comments={comments} onReply={handleReply} />
                </div>
            )}
        </div>
    );
};

ProductComment.propTypes = {
    pageId: PropTypes.string.isRequired,
    currentUser: PropTypes.shape({
        id: PropTypes.string.isRequired,
        name: PropTypes.string.isRequired,
    }).isRequired,
};

export default ProductComment;
