import BaseLayout from "../components/layout/BaseLayout";
import ProductDetailContent from "../components/contents/ProductDetailContent";
import { useParams } from "react-router-dom";
import MiddleBar from "../components/sections/MiddleBar";

function ProductDetailPage() {
    const { productId } = useParams();
    console.log("check productid:", productId);
    return (
        <BaseLayout>
            <MiddleBar />
            <ProductDetailContent productId={productId} />
        </BaseLayout>
    );
}

export default ProductDetailPage;
