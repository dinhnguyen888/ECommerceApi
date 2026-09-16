import PropTypes from "prop-types";
import { useEffect, useState } from "react";
import { useProduct } from "../../contexts/ProductContext";
import Card from "../common/Card";
import Loading from "../common/Loading";

const RelatedContent = ({ tag }) => {
    const { getProductsByTag } = useProduct();
    const [relatedProducts, setRelatedProducts] = useState([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (!tag) return;

        let isMounted = true;
        setLoading(true);

        const fetchRelatedProducts = async () => {
            try {
                const response = await getProductsByTag(tag, 1, 4);
                if (isMounted && response?.products) {
                    setRelatedProducts((prev) =>
                        JSON.stringify(prev) ===
                        JSON.stringify(response.products)
                            ? prev
                            : response.products
                    );
                }
            } catch (error) {
                console.error("Failed to fetch related products:", error);
            } finally {
                if (isMounted) setLoading(false);
            }
        };

        fetchRelatedProducts();

        return () => {
            isMounted = false;
        };
    }, [tag, getProductsByTag]);

    if (loading) return <Loading />;

    return (
        <div className="grid grid-cols-1 lg:grid-cols-2 px-16 gap-4">
            {relatedProducts.length > 0 ? (
                relatedProducts.map((product) => (
                    <Card
                        key={product.id}
                        title={product.title}
                        description={product.description}
                        imageUrl={product.imageUrl}
                        onBuy={() =>
                            console.log(`Buying product: ${product.title}`)
                        }
                        onAddToCart={() =>
                            console.log(`Adding to cart: ${product.title}`)
                        }
                        onViewDetails={() =>
                            (window.location.href = `/san-pham/${product.id}`)
                        }
                        price={product.price}
                        tag={product.tag}
                        postedDate={product.postedDate}
                    />
                ))
            ) : (
                <p className="text-gray-500">Không có sản phẩm liên quan.</p>
            )}
        </div>
    );
};

RelatedContent.propTypes = {
    tag: PropTypes.string.isRequired,
};

export default RelatedContent;
