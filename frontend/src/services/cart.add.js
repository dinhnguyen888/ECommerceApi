import apiClient from "./APIConfig";

export const addToCart = async (cartItem) => {
    try {
        const response = await apiClient.post("/Cart", {
            id: cartItem.id,
            userId: cartItem.userId,
            productId: cartItem.productId,
            productName: cartItem.productName,
            imageUrl: cartItem.imageUrl,
            price: cartItem.price,
            addToCartAt: cartItem.addToCartAt,
        });
        return response.data;
    } catch (error) {
        throw new Error(error.response?.data || "Failed to add item to cart");
    }
};
