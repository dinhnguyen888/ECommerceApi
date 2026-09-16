import { useState } from "react";
import { Comment } from "../../types/comment.type";
import {
    createComment,
    deleteComment,
    replyToComment,
} from "../../services/comment.api";
import { useAuth } from "../../context/AuthContext";

interface CommentSectionProps {
    pageId: string;
    comments: Comment[];
    onCommentUpdate: () => void;
    loading: boolean;
    productTitle?: string;
}

const buttonStyle = {
    backgroundColor: "#FFF5E6",
    border: "1px solid #ddd",
    padding: "6px 12px",
    cursor: "pointer",
    borderRadius: "4px",
    margin: "0 4px",
    color: "#333",
};

const textareaStyle = {
    width: "100%",
    minHeight: "80px",
    marginBottom: "8px",
    padding: "8px",
    borderRadius: "4px",
    border: "1px solid #ddd",
    backgroundColor: "#FFF5E6",
    color: "#333",
};

const CommentSection = ({
    pageId,
    comments,
    onCommentUpdate,
    loading,
    productTitle,
}: CommentSectionProps) => {
    const [newComment, setNewComment] = useState("");
    const [replyStates, setReplyStates] = useState<{ [key: string]: string }>(
        {}
    );
    const [replyVisibility, setReplyVisibility] = useState<{
        [key: string]: boolean;
    }>({});
    const { user } = useAuth();

    const handleSubmitComment = async () => {
        if (!newComment.trim()) return;

        try {
            await createComment({
                pageId,
                userId: user?.userId || "",
                userName: user?.userName || "Anonymous User",
                content: newComment,
            });
            setNewComment("");
            onCommentUpdate();
        } catch (error) {
            console.error("Failed to add comment:", error);
            alert("Failed to add comment");
        }
    };

    const handleReply = async (commentId: string) => {
        const replyContent = replyStates[commentId];
        if (!replyContent?.trim()) return;

        try {
            await replyToComment({
                pageId,
                commentId,
                userId: user?.userId || "",
                userName: user?.userName || "Anonymous User",
                content: replyContent,
            });
            setReplyStates({ ...replyStates, [commentId]: "" });
            setReplyVisibility({ ...replyVisibility, [commentId]: false });
            onCommentUpdate();
        } catch (error) {
            console.error("Failed to add reply:", error);
            alert("Failed to add reply");
        }
    };

    const handleDelete = async (commentId: string) => {
        if (!confirm("Are you sure you want to delete this comment?")) return;

        try {
            await deleteComment(commentId);
            onCommentUpdate();
        } catch (error) {
            console.error("Failed to delete comment:", error);
            alert("Failed to delete comment");
        }
    };

    const renderCommentItem = (comment: Comment, isReply = false) => (
        <div
            key={comment.id}
            style={{
                marginLeft: isReply ? "40px" : "0",
                marginBottom: "16px",
                borderLeft: "2px solid #eee",
                paddingLeft: "16px",
            }}
        >
            <div style={{ marginBottom: "8px" }}>
                <strong>{comment.userName}</strong>
                <span
                    style={{
                        color: "#666",
                        fontSize: "0.9em",
                        marginLeft: "8px",
                    }}
                >
                    {new Date(comment.createdAt).toLocaleString()}
                </span>
            </div>

            <p style={{ margin: "8px 0" }}>{comment.content}</p>

            <div style={{ marginTop: "8px" }}>
                <button
                    onClick={() =>
                        setReplyVisibility({
                            ...replyVisibility,
                            [comment.id]: !replyVisibility[comment.id],
                        })
                    }
                    style={buttonStyle}
                >
                    Reply
                </button>
                {user?.roleName === "Admin" && (
                    <button
                        onClick={() => handleDelete(comment.id)}
                        style={{ ...buttonStyle, color: "red" }}
                    >
                        Delete
                    </button>
                )}
            </div>

            {replyVisibility[comment.id] && (
                <div style={{ marginTop: "16px" }}>
                    <textarea
                        value={replyStates[comment.id] || ""}
                        onChange={(e) =>
                            setReplyStates({
                                ...replyStates,
                                [comment.id]: e.target.value,
                            })
                        }
                        placeholder="Write a reply..."
                        style={textareaStyle}
                    />
                    <button
                        onClick={() => handleReply(comment.id)}
                        style={buttonStyle}
                    >
                        Submit Reply
                    </button>
                </div>
            )}

            {comment.replies?.map((reply) => renderCommentItem(reply, true))}
        </div>
    );

    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <div style={{ maxWidth: "800px", margin: "0 auto", padding: "20px" }}>
            <div style={{ marginBottom: "24px" }}>
                <h2 style={{ marginBottom: "16px" }}>
                    Comments {productTitle && `for ${productTitle}`}
                </h2>
                <textarea
                    value={newComment}
                    onChange={(e) => setNewComment(e.target.value)}
                    placeholder="Write your comment..."
                    style={textareaStyle}
                />
                <button onClick={handleSubmitComment} style={buttonStyle}>
                    Add Comment
                </button>
            </div>

            <div>
                <h2 style={{ marginBottom: "16px" }}>All Comments</h2>
                {comments.length === 0 ? (
                    <p>No comments yet</p>
                ) : (
                    comments.map((comment) => renderCommentItem(comment))
                )}
            </div>
        </div>
    );
};

export default CommentSection;
