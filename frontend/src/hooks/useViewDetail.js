import { useState, useEffect } from "react";
import { fetchProductDetail } from "../services/products.api.getDetail";

const useViewDetail = (productId) => {
    const [product, setProduct] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        let isMounted = true;

        const getProductDetail = async (productId) => {
            try {
                const data = await fetchProductDetail(productId);
                if (isMounted) {
                    setProduct(data);
                }
            } catch (err) {
                if (isMounted) {
                    setError(err);
                }
            } finally {
                if (isMounted) {
                    setLoading(false);
                }
            }
        };

        if (productId) {
            getProductDetail(productId);
        }

        return () => {
            isMounted = false;
        };
    }, [productId]);

    return { product, loading, error };
};

export default useViewDetail;
