import { ICategoryRepository } from '../interfaces/ICategoryRepository';
import {
  CategoryCreateDto,
  CategoryUpdateDto,
  CategoryResponseDto
} from '../dtos/CategoryDto';
import { Category } from '../entities/Category';

export class CategoryService {
  constructor(private categoryRepository: ICategoryRepository) {}

  async getAll(): Promise<CategoryResponseDto[]> {
    const categories = await this.categoryRepository.findAll();
    return categories.map(this.toResponseDto);
  }

  async getById(id: string): Promise<CategoryResponseDto> {
    const category = await this.categoryRepository.findById(id);
    if (!category) {
      throw new Error('Category not found');
    }
    return this.toResponseDto(category);
  }

  async create(dto: CategoryCreateDto): Promise<CategoryResponseDto> {
    const category: Category = {
      ...dto,
      createdAt: new Date(),
      updatedAt: new Date()
    };
    const created = await this.categoryRepository.create(category);
    return this.toResponseDto(created);
  }

  async update(id: string, dto: CategoryUpdateDto): Promise<CategoryResponseDto> {
    const existing = await this.categoryRepository.findById(id);
    if (!existing) {
      throw new Error('Category not found');
    }
    const updated = await this.categoryRepository.update(id, dto);
    if (!updated) {
      throw new Error('Failed to update category');
    }
    return this.toResponseDto(updated);
  }

  async delete(id: string): Promise<void> {
    const deleted = await this.categoryRepository.delete(id);
    if (!deleted) {
      throw new Error('Category not found');
    }
  }

  private toResponseDto(category: Category): CategoryResponseDto {
    return {
      _id: category._id!,
      name: category.name,
      ...(category.description && { description: category.description }),
      ...(category.parentId && { parentId: category.parentId }),
      ...(category.createdAt && { createdAt: category.createdAt }),
      ...(category.updatedAt && { updatedAt: category.updatedAt })
    };
  }
}



