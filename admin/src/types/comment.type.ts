export interface CommentReply {
    id: string;
    pageId: string;
    userId: string;
    userName: string;
    content: string;
    createdAt: string;
    replies: CommentReply[];
}

export interface Comment {
    id: string;
    pageId: string;
    userId: string;
    userName: string;
    content: string;
    createdAt: string;
    replies: CommentReply[];
}

export interface CreateCommentDto {
    pageId: string;
    commentId?: string;
    userId: string;
    userName: string;
    content: string;
}
