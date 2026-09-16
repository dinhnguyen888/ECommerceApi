import PropTypes from "prop-types";
import { useState, useEffect } from "react";
import { message } from "antd";
import { getNews } from "../../services/news.get";
import ProductContent from "../contents/ProductContent";
import ArticleContent from "../contents/ArticleContent";
import NewsContent from "../contents/NewsContent";

function WrappedBody({ caseKey, caseProp }) {
    const [news, setNews] = useState([]);
    const [loading, setLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);

    useEffect(() => {
        if (caseKey === "NewsPage") {
            fetchNews();
        }
    }, [currentPage, caseKey]);

    const fetchNews = async () => {
        setLoading(true);
        try {
            const data = await getNews(currentPage, 6);
            setNews(data);
            setHasMore(data.length === 6);
        } catch (error) {
            message.error("Không thể tải tin tức: " + error.message);
        } finally {
            setLoading(false);
        }
    };

    const handleViewDetails = (linkDetail) => {
        window.open(linkDetail, "_blank");
    };

    const handlePrevPage = () => {
        if (currentPage > 1) {
            setCurrentPage((prev) => prev - 1);
        }
    };

    const handleNextPage = () => {
        if (hasMore) {
            setCurrentPage((prev) => prev + 1);
        }
    };

    let content;
    const caseItem = caseProp.find((item) => item.key === caseKey);

    if (caseItem) {
        switch (caseItem.key) {
            case "ProductPage":
                content = <ProductContent />;
                break;
            case "ArticlePage":
                content = <ArticleContent />;
                break;
            case "NewsPage":
                content = (
                    <NewsContent
                        news={news}
                        onViewDetails={handleViewDetails}
                        currentPage={currentPage}
                        onPrevPage={handlePrevPage}
                        onNextPage={handleNextPage}
                        hasMore={hasMore}
                        loading={loading}
                    />
                );
                break;
            default:
                content = <div>không tìm thấy case</div>;
                break;
        }
    }

    return <div>{content}</div>;
}

WrappedBody.propTypes = {
    caseKey: PropTypes.string.isRequired,
    caseProp: PropTypes.arrayOf(
        PropTypes.shape({
            label: PropTypes.string.isRequired,
            key: PropTypes.string.isRequired,
        })
    ).isRequired,
};

export default WrappedBody;
