import { IProductRepository } from '../../application/interfaces/IProductRepository';
import { Product } from '../../application/entities/Product';
import { ProductModel } from '../persistence/models/ProductModel';

export class ProductRepository implements IProductRepository {
  async findAll(): Promise<Product[]> {
    const docs = await ProductModel.find().exec();
    return docs.map(doc => doc.toObject());
  }

  async findById(id: string): Promise<Product | null> {
    const doc = await ProductModel.findById(id).exec();
    return doc ? doc.toObject() : null;
  }

  async create(product: Product): Promise<Product> {
    const doc = new ProductModel(product);
    await doc.save();
    return doc.toObject();
  }

  async update(id: string, product: Partial<Product>): Promise<Product | null> {
    product.updatedAt = new Date();
    const doc = await ProductModel.findByIdAndUpdate(id, product, { new: true }).exec();
    return doc ? doc.toObject() : null;
  }

  async delete(id: string): Promise<boolean> {
    const result = await ProductModel.findByIdAndDelete(id).exec();
    return result !== null;
  }
}



