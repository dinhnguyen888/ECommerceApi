import apiClient from "./APIConfig";

export const getCommentsByPageId = async (pageId) => {
    try {
        const response = await apiClient.get(`/Comment/page/${pageId}`);
        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to fetch comments");
    }
};

export const postComment = async (commentData) => {
    try {
        const formattedData = {
            pageId: commentData.pageId,
            userId: commentData.userId,
            userName: commentData.userName,
            content: commentData.content,
        };

        const response = await apiClient.post("/Comment/", formattedData);
        return response.data;
    } catch (error) {
        console.error("Post comment error:", error);
        throw new Error(error.response?.data || "Failed to post comment");
    }
};

export const replyComment = async (replyData) => {
    try {
        const formattedData = {
            pageId: replyData.pageId,
            commentId: replyData.commentId,
            userId: replyData.userId,
            userName: replyData.userName,
            content: replyData.content,
        };

        const response = await apiClient.post("/Comment/reply", formattedData);
        return response.data;
    } catch (error) {
        console.error("Reply comment error:", error);
        throw new Error(error.response?.data || "Failed to reply to comment");
    }
};
