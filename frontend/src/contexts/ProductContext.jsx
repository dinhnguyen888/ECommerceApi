import { createContext, useContext, useState, useCallback } from "react";
import PropTypes from "prop-types";
import { getProductsByTag as getProductsByTagService } from "../services/product.getByTag";
import { getMultipleProductsByTags } from "../services/product.getMultiple";
import { message } from "antd";

const ProductContext = createContext();

export const useProduct = () => {
    const context = useContext(ProductContext);
    if (!context) {
        throw new Error("useProduct must be used within a ProductProvider");
    }
    return context;
};

export const ProductProvider = ({ children }) => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(false);

    const fetchMultipleProducts = async (tagRequests) => {
        setLoading(true);
        try {
            const response = await getMultipleProductsByTags(tagRequests);
            setProducts(response);
            return response;
        } catch (error) {
            console.error("Error in context:", error);
            message.error("Không thể tải sản phẩm: " + error.message);
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const getProductsByTag = useCallback(
        async (tag, page = 1, pageSize = 4) => {
            setLoading(true);
            try {
                const response = await getProductsByTagService(
                    tag,
                    page,
                    pageSize
                );
                if (response?.products) {
                    setProducts((prevProducts) =>
                        JSON.stringify(prevProducts) ===
                        JSON.stringify(response.products)
                            ? prevProducts
                            : response.products
                    );
                }
                return response;
            } catch (error) {
                console.error("Error fetching products by tag:", error);
                message.error("Không thể tải sản phẩm: " + error.message);
                throw error;
            } finally {
                setLoading(false);
            }
        },
        []
    );

    const value = {
        products,
        loading,
        fetchMultipleProducts,
        getProductsByTag,
    };

    return (
        <ProductContext.Provider value={value}>
            {children}
        </ProductContext.Provider>
    );
};

ProductProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

export default ProductContext;
