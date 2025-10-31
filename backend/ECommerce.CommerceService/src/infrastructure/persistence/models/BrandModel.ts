import mongoose, { Schema, Document } from 'mongoose';
import { Brand } from '../../../application/entities/Brand';

const BrandSchema = new Schema<Brand & Document>({
  name: { type: String, required: true },
  description: { type: String },
  logo: { type: String },
  createdAt: { type: Date, default: Date.now },
  updatedAt: { type: Date, default: Date.now }
});

export const BrandModel = mongoose.model<Brand & Document>('Brand', BrandSchema);



