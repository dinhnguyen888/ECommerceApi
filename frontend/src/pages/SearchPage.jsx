import { useState, useEffect } from "react";
import { useSearchParams } from "react-router-dom";
import { searchProducts } from "../services/product.search";
import SearchContent from "../components/contents/SearchContent";
import { message } from "antd";
import BaseLayout from "../components/layout/BaseLayout";

import MiddleBar from "../components/sections/MiddleBar";
import Body from "../components/sections/Body";
import Filter from "../components/common/Filter";

function SearchPage() {
    const [searchParams] = useSearchParams();
    const keyword = searchParams.get("keyword") || "";
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchSearchResults = async () => {
            if (!keyword.trim()) {
                setProducts([]);
                setLoading(false);
                return;
            }

            setLoading(true);
            try {
                const data = await searchProducts(keyword);
                setProducts(data);
            } catch (error) {
                message.error("Không thể tìm kiếm sản phẩm: " + error.message);
                setProducts([]);
            } finally {
                setLoading(false);
            }
        };

        fetchSearchResults();
    }, [keyword]);

    return (
        <BaseLayout>
            <MiddleBar />
            <Body
                Content={
                    <SearchContent
                        products={products}
                        loading={loading}
                        keyword={keyword}
                    />
                }
                Sidebar={<Filter />}
            />
        </BaseLayout>
    );
}

export default SearchPage;
