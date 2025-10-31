import { ICategoryRepository } from '../../application/interfaces/ICategoryRepository';
import { Category } from '../../application/entities/Category';
import { CategoryModel } from '../persistence/models/CategoryModel';

export class CategoryRepository implements ICategoryRepository {
  async findAll(): Promise<Category[]> {
    const docs = await CategoryModel.find().exec();
    return docs.map(doc => doc.toObject());
  }

  async findById(id: string): Promise<Category | null> {
    const doc = await CategoryModel.findById(id).exec();
    return doc ? doc.toObject() : null;
  }

  async create(category: Category): Promise<Category> {
    const doc = new CategoryModel(category);
    await doc.save();
    return doc.toObject();
  }

  async update(id: string, category: Partial<Category>): Promise<Category | null> {
    category.updatedAt = new Date();
    const doc = await CategoryModel.findByIdAndUpdate(id, category, { new: true }).exec();
    return doc ? doc.toObject() : null;
  }

  async delete(id: string): Promise<boolean> {
    const result = await CategoryModel.findByIdAndDelete(id).exec();
    return result !== null;
  }
}



