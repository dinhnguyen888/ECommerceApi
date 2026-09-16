import { createContext, useState, useContext, useEffect } from "react";
import PropTypes from "prop-types";
import { getCartByUserId } from "../services/cart.getById";
import { useAuth } from "./AuthContext";
import { addToCart as addToCartService } from "../services/cart.add";
import { removeFromCart as removeFromCartService } from "../services/cart.remove";
import { clearCart as clearCartService } from "../services/cart.clear";
import toast from "react-hot-toast";

const CartContext = createContext();

export const CartProvider = ({ children }) => {
    const { userId } = useAuth();
    const [cartItems, setCartItems] = useState([]);

    useEffect(() => {
        const fetchCartItems = async () => {
            try {
                const items = await getCartByUserId(userId);
                setCartItems(Array.isArray(items) ? items : []);
            } catch (error) {
                console.error("Failed to fetch cart items:", error);
            }
        };

        if (userId) {
            fetchCartItems();
        }
    }, [userId]);

    const addToCart = async (cartItem) => {
        if (cartItems.some((item) => item.productId === cartItem.productId)) {
            console.warn("Item already in cart");
            toast.success("trong giỏ hàng đã có sản phẩm này");
            return;
        }
        try {
            const newItem = await addToCartService(cartItem);
            setCartItems([...cartItems, newItem]);
            toast.success("đã thêm vào giỏ hàng");
        } catch (error) {
            console.error("Failed to add item to cart:", error);
        }
    };

    const removeFromCart = async (id) => {
        try {
            await removeFromCartService(id);
            setCartItems(cartItems.filter((item) => item.id !== id));
        } catch (error) {
            console.error("Failed to remove item from cart:", error);
        }
    };

    const clearCart = async (userId) => {
        toast((t) => (
            <span>
                Bạn có chắc chắn muốn xóa giỏ hàng không?
                <div className="flex justify-center text-xs gap-4 mt-4">
                    <button
                        onClick={async () => {
                            try {
                                await clearCartService(userId);
                                setCartItems([]);
                                toast.dismiss(t.id);
                            } catch (error) {
                                console.error("Failed to clear cart:", error);
                            }
                        }}
                    >
                        Xóa
                    </button>
                    <button onClick={() => toast.dismiss(t.id)}>Hủy</button>
                </div>
            </span>
        ));
    };

    return (
        <CartContext.Provider
            value={{
                cartItems,
                setCartItems,
                addToCart,
                removeFromCart,
                clearCart,
            }}
        >
            {children}
        </CartContext.Provider>
    );
};

CartProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

export const useCart = () => useContext(CartContext);
