import { IBrandRepository } from '../../application/interfaces/IBrandRepository';
import { Brand } from '../../application/entities/Brand';
import { BrandModel } from '../persistence/models/BrandModel';

export class BrandRepository implements IBrandRepository {
  async findAll(): Promise<Brand[]> {
    const docs = await BrandModel.find().exec();
    return docs.map(doc => doc.toObject());
  }

  async findById(id: string): Promise<Brand | null> {
    const doc = await BrandModel.findById(id).exec();
    return doc ? doc.toObject() : null;
  }

  async create(brand: Brand): Promise<Brand> {
    const doc = new BrandModel(brand);
    await doc.save();
    return doc.toObject();
  }

  async update(id: string, brand: Partial<Brand>): Promise<Brand | null> {
    brand.updatedAt = new Date();
    const doc = await BrandModel.findByIdAndUpdate(id, brand, { new: true }).exec();
    return doc ? doc.toObject() : null;
  }

  async delete(id: string): Promise<boolean> {
    const result = await BrandModel.findByIdAndDelete(id).exec();
    return result !== null;
  }
}



