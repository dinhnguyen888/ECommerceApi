import PropTypes from "prop-types";
import HorizontalCard from "../common/HorizontalCard";
import { Button } from "antd";
import { LeftOutlined, RightOutlined } from "@ant-design/icons";
import Loading from "../common/Loading";
import MainTitle from "../common/MainTitle";
const NewsContent = ({
    news,
    onViewDetails,
    currentPage,
    onPrevPage,
    onNextPage,
    hasMore,
    loading,
}) => {
    return (
        <div className="container mx-auto px-16 py-8">
            <MainTitle
                text="Tin tức công nghệ"
                className={"text-balance uppercase my-7 font-semibold"}
            />
            <div className="relative min-h-[400px]">
                {loading && (
                    <div className="absolute inset-0 bg-white bg-opacity-80 z-10">
                        <Loading />
                    </div>
                )}
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                    {news.map((item) => (
                        <HorizontalCard
                            key={item.id}
                            title={item.title}
                            description={item.description}
                            imageUrl={item.imageUrl}
                            tag={item.type.replace("/", "")}
                            postedDate={new Date().toISOString()}
                            onViewDetails={() => onViewDetails(item.linkDetail)}
                        />
                    ))}
                </div>
            </div>
            <div className="mt-8 flex justify-center gap-4">
                <Button
                    onClick={onPrevPage}
                    disabled={currentPage === 1 || loading}
                    icon={<LeftOutlined className="h-4 w-4" />}
                >
                    Trang trước
                </Button>
                <Button
                    onClick={onNextPage}
                    disabled={!hasMore || loading}
                    icon={<RightOutlined className="h-4 w-4" />}
                >
                    Trang sau
                </Button>
            </div>
        </div>
    );
};

NewsContent.propTypes = {
    news: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.string.isRequired,
            title: PropTypes.string.isRequired,
            description: PropTypes.string.isRequired,
            imageUrl: PropTypes.string.isRequired,
            type: PropTypes.string.isRequired,
            linkDetail: PropTypes.string.isRequired,
        })
    ).isRequired,
    onViewDetails: PropTypes.func.isRequired,
    currentPage: PropTypes.number.isRequired,
    onPrevPage: PropTypes.func.isRequired,
    onNextPage: PropTypes.func.isRequired,
    hasMore: PropTypes.bool.isRequired,
    loading: PropTypes.bool.isRequired,
};

export default NewsContent;
