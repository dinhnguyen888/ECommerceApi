export interface News {
    id: string;
    title: string;
    linkDetail: string;
    imageUrl: string;
    type: string;
    description: string;
}

export interface NewsRequest {
    id?: string;
    title: string;
    linkDetail: string;
    imageUrl: string;
    type: string;
    description: string;
}

export interface NewsPagingRequest {
    pageNumber: number;
    pageSize: number;
}
