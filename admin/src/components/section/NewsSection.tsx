import { Button, Card, Tooltip } from "antd";
import {
    ReloadOutlined,
    EditOutlined,
    EyeOutlined,
    DeleteOutlined,
} from "@ant-design/icons";
import { News } from "../../types/news.type";
import newsApi from "../../services/news.api";
import BaseTable from "../common/BaseTable";
import { ColumnsType } from "antd/es/table";
import { PaginationMeta } from "../../types/base.type";
import NewsEditModal from "../modal/news/NewsEditModal";

interface NewsData extends PaginationMeta {
    news: News[];
    totalNews: number;
}

interface NewsSectionProps {
    data: NewsData;
    loading: boolean;
    onPageChange: (page: number, pageSize: number) => void;
    refreshData: () => void;
    modalLoading: boolean;
    editModalOpen: boolean;
    selectedNews: News | null;
    onEditModalClose: () => void;
    onEdit: (values: News) => Promise<void>;
    onEditClick: (id: string) => void;
    onDelete: (id: string) => Promise<void>;
}

const NewsSection = ({
    data,
    loading,
    onPageChange,
    refreshData,
    modalLoading,
    editModalOpen,
    selectedNews,
    onEditModalClose,
    onEdit,
    onEditClick,
    onDelete,
}: NewsSectionProps) => {
    const handleCrawlNews = async () => {
        try {
            await newsApi.crawlNews(1);
            refreshData();
        } catch (error) {
            console.error("Error crawling news:", error);
        }
    };

    const handleCrawlLatest = async () => {
        try {
            await newsApi.crawlLatestNews();
            refreshData();
        } catch (error) {
            console.error("Error crawling latest news:", error);
        }
    };

    const columns: ColumnsType<News> = [
        {
            title: "Image",
            dataIndex: "imageUrl",
            key: "imageUrl",
            width: 100,
            render: (imageUrl: string) => (
                <img
                    src={imageUrl || "https://placehold.co/100x60"}
                    alt="news"
                    style={{
                        width: "80px",
                        height: "48px",
                        objectFit: "cover",
                        borderRadius: "4px",
                    }}
                />
            ),
        },
        {
            title: "Title",
            dataIndex: "title",
            key: "title",
            width: 180,
            ellipsis: {
                showTitle: false,
            },
            render: (text: string) => (
                <Tooltip placement="topLeft" title={text}>
                    <strong>{text}</strong>
                </Tooltip>
            ),
        },
        {
            title: "Type",
            dataIndex: "type",
            key: "type",
            width: 120,
            render: (type: string) => (
                <Tooltip placement="topLeft" title={type}>
                    {type.replace("/", " > ")}
                </Tooltip>
            ),
        },
        {
            title: "Description",
            dataIndex: "description",
            key: "description",
            width: 300,
            ellipsis: {
                showTitle: false,
            },
            render: (text: string) => (
                <Tooltip placement="topLeft" title={text}>
                    {text}
                </Tooltip>
            ),
        },
        {
            title: "Actions",
            key: "actions",
            width: 100,
            fixed: "right",
            render: (_, record) => (
                <Button.Group>
                    <Tooltip title="View Details">
                        <Button
                            icon={<EyeOutlined />}
                            onClick={() =>
                                window.open(record.linkDetail, "_blank")
                            }
                            type="text"
                        />
                    </Tooltip>
                    <Tooltip title="Edit">
                        <Button
                            icon={<EditOutlined />}
                            onClick={() => onEditClick(record.id)}
                            type="text"
                        />
                    </Tooltip>
                    <Tooltip title="Delete">
                        <Button
                            icon={<DeleteOutlined />}
                            onClick={() => onDelete(record.id)}
                            type="text"
                            danger
                        />
                    </Tooltip>
                </Button.Group>
            ),
        },
    ];

    return (
        <Card
            title="News Management"
            extra={
                <div style={{ display: "flex", gap: "1rem" }}>
                    <Button
                        type="primary"
                        onClick={handleCrawlNews}
                        icon={<ReloadOutlined />}
                        loading={loading}
                    >
                        Crawl News
                    </Button>
                    <Button
                        onClick={handleCrawlLatest}
                        icon={<ReloadOutlined />}
                        loading={loading}
                    >
                        Crawl Latest
                    </Button>
                </div>
            }
        >
            <BaseTable<News, NewsData>
                columns={columns}
                data={data}
                loading={loading}
                onPageChange={onPageChange}
                dataSource={data.news}
                rowKey="id"
                scroll={{ x: 990 }}
            />

            {selectedNews && (
                <NewsEditModal
                    open={editModalOpen}
                    onCancel={onEditModalClose}
                    onSubmit={onEdit}
                    loading={modalLoading}
                    news={selectedNews}
                />
            )}
        </Card>
    );
};

export default NewsSection;
