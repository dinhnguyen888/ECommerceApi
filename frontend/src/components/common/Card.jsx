import PropTypes from "prop-types";
import { Card as AntCard, Button } from "antd";
import { ShoppingCartOutlined, DollarOutlined } from "@ant-design/icons";

function Card({
    title,
    description,
    imageUrl,
    onBuy,
    onAddToCart,
    onViewDetails,
    price,
    tag,
    postedDate,
}) {
    const handleBuyClick = (e) => {
        e.stopPropagation();
        onBuy();
    };

    const handleAddToCartClick = (e) => {
        e.stopPropagation();
        onAddToCart();
    };

    return (
        <AntCard
            hoverable
            className="w-full overflow-hidden cursor-pointer"
            cover={
                <div className="relative">
                    <img
                        src={imageUrl}
                        alt={title}
                        className="w-full h-44 md:h-44 h-32 object-cover rounded-t-md"
                    />
                    <p className="absolute bottom-0 bg-black opacity-50 w-full text-red-200 md:text-sm text-xs text-center md:py-1 py-0.5">
                        Giá: {price.toLocaleString()} đ
                    </p>
                </div>
            }
            onClick={onViewDetails}
            bodyStyle={{ padding: "12px" }}
        >
            <AntCard.Meta
                title={
                    <h2 className="md:text-md text-sm font-semibold text-center truncate">
                        {title}
                    </h2>
                }
                description={
                    <div className="text-gray-700 text-xs">
                        <span className="font-semibold truncate text-blue-800 block">
                            #{tag}
                        </span>
                        <p className="line-clamp-1 md:line-clamp-2 overflow-hidden text-ellipsis">
                            {description}
                        </p>
                        <p className="text-gray-500 text-xs mt-1 md:mt-2">
                            {new Date(postedDate).toLocaleDateString()}
                        </p>
                    </div>
                }
            />
            <div className="flex justify-center gap-2 mt-2 md:mt-4">
                <Button
                    type="primary"
                    icon={<DollarOutlined />}
                    size="middle"
                    className="flex items-center min-w-[100px] md:min-w-[120px] justify-center hover:opacity-90"
                    onClick={handleBuyClick}
                >
                    <span className="md:text-sm text-xs">Mua ngay</span>
                </Button>
                <Button
                    type="default"
                    icon={<ShoppingCartOutlined />}
                    size="middle"
                    className="flex items-center min-w-[100px] md:min-w-[120px] justify-center border-blue-500 text-blue-500 hover:text-blue-600 hover:border-blue-600"
                    onClick={handleAddToCartClick}
                >
                    <span className="md:text-sm text-xs">Thêm vào giỏ</span>
                </Button>
            </div>
        </AntCard>
    );
}

Card.propTypes = {
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    imageUrl: PropTypes.string.isRequired,
    onBuy: PropTypes.func.isRequired,
    onAddToCart: PropTypes.func.isRequired,
    onViewDetails: PropTypes.func.isRequired,
    price: PropTypes.number.isRequired,
    tag: PropTypes.string,
    postedDate: PropTypes.string.isRequired,
};

export default Card;
