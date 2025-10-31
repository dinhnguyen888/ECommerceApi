import mongoose, { Schema, Document } from 'mongoose';
import { Category } from '../../../application/entities/Category';

const CategorySchema = new Schema<Category & Document>({
  name: { type: String, required: true },
  description: { type: String },
  parentId: { type: String },
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

export const CategoryModel = mongoose.model<Category & Document>('Category', CategorySchema);



