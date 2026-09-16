import { Button, Space, Table, Tooltip, message } from "antd";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { useCallback, useEffect, useState } from "react";
import { Banner } from "../../types/banner.type";
import { deleteBanner, getBanners } from "../../services/banner.api";
import { BannerAddModal } from "../modal/banner/BannerAddModal";
import { BannerEditModal } from "../modal/banner/BannerEditModal";

export const BannerSection = () => {
    const [banners, setBanners] = useState<Banner[]>([]);
    const [loading, setLoading] = useState(false);
    const [isAddModalOpen, setIsAddModalOpen] = useState(false);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [selectedBanner, setSelectedBanner] = useState<Banner | null>(null);

    const fetchBanners = useCallback(async () => {
        try {
            setLoading(true);
            const data = await getBanners();
            setBanners(data);
        } catch (error: Error | unknown) {
            message.error(
                error instanceof Error
                    ? error.message
                    : "Failed to fetch banners"
            );
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchBanners();
    }, [fetchBanners]);

    const handleDelete = async (id: string) => {
        try {
            await deleteBanner(id);
            message.success("Banner deleted successfully");
            fetchBanners();
        } catch (error: Error | unknown) {
            message.error(
                error instanceof Error
                    ? error.message
                    : "Failed to delete banner"
            );
        }
    };

    const columns = [
        {
            title: "Banner Name",
            dataIndex: "bannerName",
            key: "bannerName",
        },
        {
            title: "Banner Image",
            dataIndex: "bannerUrl",
            key: "bannerUrl",
            render: (url: string) => (
                <img
                    src={url}
                    alt="Banner"
                    style={{ width: 100, height: 60, objectFit: "cover" }}
                />
            ),
        },
        {
            title: "Actions",
            key: "actions",
            render: (_: unknown, record: Banner) => (
                <Space>
                    <Tooltip title="Edit">
                        <Button
                            type="primary"
                            icon={<EditOutlined />}
                            onClick={() => {
                                setSelectedBanner(record);
                                setIsEditModalOpen(true);
                            }}
                        />
                    </Tooltip>
                    <Tooltip title="Delete">
                        <Button
                            danger
                            icon={<DeleteOutlined />}
                            onClick={() => handleDelete(record.id)}
                        />
                    </Tooltip>
                </Space>
            ),
        },
    ];

    return (
        <div>
            <div style={{ marginBottom: 16 }}>
                <Button
                    type="primary"
                    icon={<PlusOutlined />}
                    onClick={() => setIsAddModalOpen(true)}
                >
                    Add Banner
                </Button>
            </div>

            <Table
                columns={columns}
                dataSource={banners}
                loading={loading}
                rowKey="id"
            />

            <BannerAddModal
                isOpen={isAddModalOpen}
                onClose={() => setIsAddModalOpen(false)}
                onSuccess={fetchBanners}
            />

            {selectedBanner && (
                <BannerEditModal
                    isOpen={isEditModalOpen}
                    onClose={() => {
                        setIsEditModalOpen(false);
                        setSelectedBanner(null);
                    }}
                    onSuccess={fetchBanners}
                    banner={selectedBanner}
                />
            )}
        </div>
    );
};
