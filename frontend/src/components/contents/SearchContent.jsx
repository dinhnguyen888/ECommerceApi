import PropTypes from "prop-types";
import Card from "../common/Card";
import Loading from "../common/Loading";
import { Empty } from "antd";
import MainTitle from "../common/MainTitle";

const SearchContent = ({ products, loading, keyword }) => {
    if (loading) {
        return (
            <div className="min-h-[400px] relative">
                <Loading />
            </div>
        );
    }

    if (!products.length) {
        return (
            <div className="min-h-[400px] flex items-center justify-center">
                <Empty
                    description={
                        <span>
                            Không tìm thấy sản phẩm nào cho từ khóa &quot;
                            {keyword}&quot;
                        </span>
                    }
                />
            </div>
        );
    }

    return (
        <div className="container mx-auto px-16 py-8">
            <MainTitle
                text={`Kết quả tìm kiếm cho: "${keyword}"`}
                className={"text-3xl font-semibold mb-8"}
            />
            <div className="grid grid-cols-1 sm:grid-cols-2  lg:grid-cols-2 gap-6">
                {products.map((product) => (
                    <Card
                        key={product.id}
                        title={product.productName}
                        description={product.description}
                        imageUrl={product.imageUrl}
                        price={product.price}
                        tag={product.tag}
                        postedDate={product.postedDate}
                        onBuy={() => {}}
                        onAddToCart={() => {}}
                        onViewDetails={() => {}}
                    />
                ))}
            </div>
        </div>
    );
};

SearchContent.propTypes = {
    products: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.number.isRequired,
            productName: PropTypes.string.isRequired,
            description: PropTypes.string.isRequired,
            imageUrl: PropTypes.string.isRequired,
            price: PropTypes.number.isRequired,
            tag: PropTypes.string,
            postedDate: PropTypes.string.isRequired,
        })
    ).isRequired,
    loading: PropTypes.bool.isRequired,
    keyword: PropTypes.string.isRequired,
};

export default SearchContent;
