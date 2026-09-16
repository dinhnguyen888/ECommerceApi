import { useState, useEffect } from "react";
import Banner from "../common/Banner";
import Card from "../common/Card";
import { message } from "antd";
import Loading from "../common/Loading";
import { useAuth } from "../../contexts/AuthContext";
import { useCart } from "../../contexts/CartContext";
import { usePayment } from "../../contexts/PaymentContext";
import { useProduct } from "../../contexts/ProductContext";
import { getBanner } from "../../services/banner.get";
import MainTitle from "../common/MainTitle";

const SECTIONS = [
    {
        title: "Web App",
        tag: "webapp",
        limit: 2,
    },
    {
        title: "Phần mềm Desktop",
        tag: "phanmem",
        limit: 4,
    },
    {
        title: "Công cụ & Tiện ích",
        tag: "tool",
        limit: 3,
    },
];

function ProductContent() {
    const { products, loading, fetchMultipleProducts } = useProduct();
    const { addToCart } = useCart();
    const { userId } = useAuth();
    const { navigatePayment } = usePayment();
    const [banners, setBanners] = useState([]);

    useEffect(() => {
        const fetchBanner = async () => {
            try {
                const banners = await getBanner();
                setBanners(banners);
            } catch (error) {
                console.error("Failed to fetch banner", error);
            }
        };
        fetchBanner();
    }, []);

    useEffect(() => {
        fetchMultipleProducts(
            SECTIONS.map(({ tag, limit }) => ({ tag, limit }))
        );
    }, []);

    const handleBuyClick = (product) => {
        navigatePayment(product.id, {
            productPay: product.title,
            productPrice: product.price,
        });
    };

    const handleAddToCart = async (product) => {
        try {
            await addToCart({
                id: crypto.randomUUID(),
                userId,
                productId: product.id,
                productName: product.title,
                productDescription: product.description,
                imageUrl: product.imageUrl,
                price: product.price,
                addToCartAt: new Date().toISOString(),
            });
            message.success(`Đã thêm ${product.title} vào giỏ hàng`);
        } catch (error) {
            console.error("Failed to add product to cart", error);
            message.error("Không thể thêm vào giỏ hàng");
        }
    };

    const handleViewDetail = (product) => {
        window.location.href = `/san-pham/${product.id}`;
    };

    const handleViewMore = (tag) => {
        window.location.href = `/danh-muc/${tag}`;
    };

    // Lọc sản phẩm theo tag
    const getProductsByTag = (tag) => {
        return products.filter((product) => product.tag === tag);
    };

    return (
        <div className="container mx-auto p-4 mt-5 lg:px-20">
            <div className="lg:">
                <Banner images={banners} />
            </div>

            <div className="mt-8 space-y-12">
                {SECTIONS.map((section) => {
                    const sectionProducts = getProductsByTag(section.tag);

                    return (
                        <div key={section.tag} className="my-5">
                            <div className="flex justify-between items-center my-11">
                                <MainTitle
                                    text={section.title}
                                    className=" uppercase font-semibold m-0"
                                />
                                {sectionProducts.length > 0 && (
                                    <span
                                        onClick={() =>
                                            handleViewMore(section.tag)
                                        }
                                        className="text-blue-600 hover:text-blue-800 cursor-pointer text-sm font-medium mr-2 hover:underline"
                                    >
                                        Xem thêm
                                    </span>
                                )}
                            </div>
                            {loading ? (
                                <div className="flex justify-center">
                                    <Loading />
                                </div>
                            ) : sectionProducts.length === 0 ? (
                                <div className="text-center text-gray-500">
                                    Không có sản phẩm nào trong danh mục này
                                </div>
                            ) : (
                                <div className="grid grid-cols-1 md:grid-cols-2 gap-7 px-5 lg:px-0 justify-items-center">
                                    {sectionProducts.map((product) => (
                                        <Card
                                            key={product.id}
                                            imageUrl={product.imageUrl}
                                            title={product.title}
                                            description={product.description}
                                            onBuy={() =>
                                                handleBuyClick(product)
                                            }
                                            onAddToCart={() =>
                                                handleAddToCart(product)
                                            }
                                            onViewDetails={() =>
                                                handleViewDetail(product)
                                            }
                                            price={product.price}
                                            tag={product.tag}
                                            postedDate={product.postedDate}
                                        />
                                    ))}
                                </div>
                            )}
                        </div>
                    );
                })}
            </div>
        </div>
    );
}

export default ProductContent;
