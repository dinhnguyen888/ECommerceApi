import { News, NewsPagingRequest, NewsRequest } from "../types/news.type";
import { axiosInstance } from "./apiConfig";

const newsApi = {
    getNews: async ({ pageNumber, pageSize }: NewsPagingRequest) => {
        try {
            const response = await axiosInstance.get<News[]>(
                `/News?pageNumber=${pageNumber}&pageSize=${pageSize}`
            );
            return response.data;
        } catch (error) {
            console.error("Error fetching news:", error);
            return [];
        }
    },

    crawlNews: async (totalCrawlingPage: number) => {
        try {
            const response = await axiosInstance.get(
                `/News/crawl?totalCrawlingPage=${totalCrawlingPage}`
            );
            return response.data;
        } catch (error) {
            console.error("Error crawling news:", error);
            throw error;
        }
    },

    crawlLatestNews: async () => {
        try {
            const response = await axiosInstance.get("/News/latest");
            return response.data;
        } catch (error) {
            console.error("Error crawling latest news:", error);
            throw error;
        }
    },

    updateNews: async (newsData: NewsRequest) => {
        try {
            const response = await axiosInstance.put(
                `/News/${newsData.id}`,
                newsData
            );
            return response.data;
        } catch (error) {
            console.error("Error updating news:", error);
            throw error;
        }
    },

    deleteNews: async (id: string) => {
        try {
            const response = await axiosInstance.delete(`/News/${id}`);
            return response.data;
        } catch (error) {
            console.error("Error deleting news:", error);
            throw error;
        }
    },
};

export default newsApi;
