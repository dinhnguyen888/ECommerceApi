import mongoose, { Schema, Document } from 'mongoose';
import { Product } from '../../../application/entities/Product';

const ProductSchema = new Schema<Product & Document>({
  name: { type: String, required: true },
  description: { type: String, required: true },
  price: { type: Number, required: true },
  stock: { type: Number, required: true },
  brandId: { type: String, required: true },
  categoryId: { type: String, required: true },
  images: [{ type: String }],
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

export const ProductModel = mongoose.model<Product & Document>('Product', ProductSchema);



