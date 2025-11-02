import mongoose, { Schema, Document } from 'mongoose';
import { Cart, CartItem } from '../../../application/entities/Cart';

const CartItemSchema = new Schema({
  productId: { type: String, required: true },
  quantity: { type: Number, required: true },
  price: { type: Number, required: true }
}, { _id: false });

interface CartDocument extends Omit<Cart, '_id'>, Document {}

const CartSchema = new Schema<CartDocument>({
  userId: { type: String, required: true, unique: true },
  items: [CartItemSchema],
  totalAmount: { type: Number, default: 0 },
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

export const CartModel = mongoose.model<CartDocument>('Cart', CartSchema);



