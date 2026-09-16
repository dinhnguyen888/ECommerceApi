import BaseLayout from "../components/layout/BaseLayout";
import ProductContent from "../components/contents/ProductContent";
import MiddleBar from "../components/sections/MiddleBar";

function ProductPage() {
    return (
        <BaseLayout>
            <MiddleBar />
            <ProductContent />
        </BaseLayout>
    );
}

export default ProductPage;
