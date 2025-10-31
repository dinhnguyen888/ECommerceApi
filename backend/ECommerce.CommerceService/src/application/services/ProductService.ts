import { IProductRepository } from '../interfaces/IProductRepository';
import {
  ProductCreateDto,
  ProductUpdateDto,
  ProductResponseDto
} from '../dtos/ProductDto';
import { Product } from '../entities/Product';

export class ProductService {
  constructor(private productRepository: IProductRepository) {}

  async getAll(): Promise<ProductResponseDto[]> {
    const products = await this.productRepository.findAll();
    return products.map(this.toResponseDto);
  }

  async getById(id: string): Promise<ProductResponseDto> {
    const product = await this.productRepository.findById(id);
    if (!product) {
      throw new Error('Product not found');
    }
    return this.toResponseDto(product);
  }

  async create(dto: ProductCreateDto): Promise<ProductResponseDto> {
    const product: Product = {
      ...dto,
      createdAt: new Date(),
      updatedAt: new Date()
    };
    const created = await this.productRepository.create(product);
    return this.toResponseDto(created);
  }

  async update(id: string, dto: ProductUpdateDto): Promise<ProductResponseDto> {
    const existing = await this.productRepository.findById(id);
    if (!existing) {
      throw new Error('Product not found');
    }
    const updated = await this.productRepository.update(id, dto);
    if (!updated) {
      throw new Error('Failed to update product');
    }
    return this.toResponseDto(updated);
  }

  async delete(id: string): Promise<void> {
    const deleted = await this.productRepository.delete(id);
    if (!deleted) {
      throw new Error('Product not found');
    }
  }

  private toResponseDto(product: Product): ProductResponseDto {
    return {
      _id: product._id!,
      name: product.name,
      description: product.description,
      price: product.price,
      stock: product.stock,
      brandId: product.brandId,
      categoryId: product.categoryId,
      ...(product.images && { images: product.images }),
      ...(product.createdAt && { createdAt: product.createdAt }),
      ...(product.updatedAt && { updatedAt: product.updatedAt })
    };
  }
}



