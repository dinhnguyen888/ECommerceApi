import { useState, useEffect } from "react";
import { message, Modal } from "antd";
import BaseLayout from "../components/layout/BaseLayout";
import NewsSection from "../components/section/NewsSection";
import { News } from "../types/news.type";
import newsApi from "../services/news.api";
import { PaginationParams } from "../types/base.type";
import { ExclamationCircleOutlined } from "@ant-design/icons";

const { confirm } = Modal;

const initialPagination: PaginationParams = {
    page: 1,
    pageSize: 10,
};

const NewsPage = () => {
    const [loading, setLoading] = useState(false);
    const [modalLoading, setModalLoading] = useState(false);
    const [editModalOpen, setEditModalOpen] = useState(false);
    const [selectedNews, setSelectedNews] = useState<News | null>(null);
    const [data, setData] = useState({
        currentPage: initialPagination.page,
        pageSize: initialPagination.pageSize,
        totalNews: 0,
        totalPages: 1,
        news: [] as News[],
    });

    const fetchNews = async (pageSize: number = data.pageSize) => {
        try {
            setLoading(true);
            const newsData = await newsApi.getNews({
                pageNumber: 1, // Always fetch first page
                pageSize: pageSize,
            });

            setData({
                currentPage: 1,
                pageSize: pageSize,
                news: newsData,
                totalNews: newsData.length,
                totalPages: 1,
            });
        } catch (error) {
            console.error("Error fetching news:", error);
            message.error("Cannot load news list!");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchNews();
    }, []);

    const handlePageSizeChange = (_: number, pageSize: number) => {
        fetchNews(pageSize);
    };

    const handleEdit = async (values: News) => {
        if (!selectedNews?.id) return;
        setModalLoading(true);
        try {
            await newsApi.updateNews({
                ...selectedNews,
                ...values,
                id: selectedNews.id,
            });
            message.success("News updated successfully!");
            setEditModalOpen(false);
            setSelectedNews(null);
            fetchNews();
        } catch (error) {
            console.error("Update news error:", error);
            message.error("Failed to update news!");
        } finally {
            setModalLoading(false);
        }
    };

    const handleDelete = async (id: string) => {
        confirm({
            title: "Are you sure you want to delete this news?",
            icon: <ExclamationCircleOutlined />,
            content: "This action cannot be undone.",
            okText: "Yes",
            okType: "danger",
            cancelText: "No",
            onOk: async () => {
                try {
                    await newsApi.deleteNews(id);
                    message.success("News deleted successfully!");
                    fetchNews();
                } catch (error) {
                    console.error("Delete news error:", error);
                    message.error("Failed to delete news!");
                }
            },
        });
    };

    const handleEditClick = async (id: string) => {
        const newsToEdit = data.news.find((news) => news.id === id);
        if (newsToEdit) {
            setSelectedNews(newsToEdit);
            setEditModalOpen(true);
        }
    };

    return (
        <BaseLayout>
            <NewsSection
                data={data}
                loading={loading}
                modalLoading={modalLoading}
                editModalOpen={editModalOpen}
                selectedNews={selectedNews}
                onEditModalClose={() => {
                    setEditModalOpen(false);
                    setSelectedNews(null);
                }}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onEditClick={handleEditClick}
                onPageChange={handlePageSizeChange}
                refreshData={fetchNews}
            />
        </BaseLayout>
    );
};

export default NewsPage;
