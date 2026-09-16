import { Comment, CreateCommentDto } from "../types/comment.type";
import { axiosInstance } from "./apiConfig";

const COMMENT_URL = "/Comment";

export const getComments = async (
    pageId: string,
    pageNumber: number = 1,
    pageSize: number = 10
): Promise<Comment[]> => {
    const response = await axiosInstance.get(
        `${COMMENT_URL}/page/${pageId}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
    return response.data;
};

export const createComment = async (
    comment: CreateCommentDto
): Promise<void> => {
    await axiosInstance.post(COMMENT_URL, comment);
};

export const replyToComment = async (
    reply: CreateCommentDto
): Promise<void> => {
    await axiosInstance.post(`${COMMENT_URL}/reply`, reply);
};

export const deleteComment = async (commentId: string): Promise<void> => {
    await axiosInstance.delete(`${COMMENT_URL}/${commentId}`);
};
