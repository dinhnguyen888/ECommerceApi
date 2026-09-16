import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Comment } from "../types/comment.type";
import { getComments } from "../services/comment.api";
import { productApi } from "../services/product.api";
import CommentSection from "../components/section/CommentSection";
import BaseLayout from "../components/layout/BaseLayout";
import { Alert } from "antd";

const CommentPage = () => {
    const { productId } = useParams<{ productId: string }>();
    const [comments, setComments] = useState<Comment[]>([]);
    const [productTitle, setProductTitle] = useState<string>("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchProductDetails = async () => {
        try {
            if (!productId) return;
            const product = await productApi.getProductDetail(productId);
            setProductTitle(product.title);
        } catch (err) {
            console.error("Failed to fetch product details:", err);
        }
    };

    const fetchComments = async () => {
        try {
            if (!productId) {
                throw new Error("Product ID is required");
            }
            setLoading(true);
            const data = await getComments(productId);
            setComments(data);
            setError(null);
        } catch (err) {
            console.error("Failed to fetch comments:", err);
            setError("Failed to load comments");
        } finally {
            setLoading(false);
        }
    };

    const handleCommentUpdate = () => {
        fetchComments();
    };

    useEffect(() => {
        if (productId) {
            fetchComments();
            fetchProductDetails();
        }
    }, [productId]);

    return (
        <BaseLayout>
            <div className="p-6">
                {/* <h1 className="text-2xl font-bold mb-6">Comments</h1> */}
                {error && (
                    <Alert type="error" message={error} className="mb-4" />
                )}
                <CommentSection
                    pageId={productId || ""}
                    comments={comments}
                    onCommentUpdate={handleCommentUpdate}
                    loading={loading}
                    productTitle={productTitle}
                />
            </div>
        </BaseLayout>
    );
};

export default CommentPage;
