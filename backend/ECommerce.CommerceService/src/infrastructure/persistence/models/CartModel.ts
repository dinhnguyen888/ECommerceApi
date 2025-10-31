import mongoose, { Schema, Document } from 'mongoose';
import { Cart, CartItem } from '../../../application/entities/Cart';

const CartItemSchema = new Schema<CartItem>({
  productId: { type: String, required: true },
  quantity: { type: Number, required: true },
  price: { type: Number, required: true }
}, { _id: false });

const CartSchema = new Schema<Cart & Document>({
  userId: { type: String, required: true, unique: true },
  items: [CartItemSchema],
  totalAmount: { type: Number, default: 0 },
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

export const CartModel = mongoose.model<Cart & Document>('Cart', CartSchema);



