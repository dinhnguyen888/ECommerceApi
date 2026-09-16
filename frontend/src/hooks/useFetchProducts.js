import { useState, useEffect } from "react";
import { fetchProducts } from "../services/products.get.api";

const useFetchProducts = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const getProducts = async () => {
            try {
                const data = await fetchProducts();
                setProducts(data);
            } catch (err) {
                setError(err);
            } finally {
                setLoading(false);
            }
        };

        getProducts();
    }, []);

    return { products, loading, error };
};

export default useFetchProducts;
