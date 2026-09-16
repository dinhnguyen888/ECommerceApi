export interface CartItem {
    id: string;
    userId: string;
    productName: string;
    productId: string;
    imageUrl: string;
    price: number;
    addToCartAt: string;
}

export type CartItems = CartItem[];
