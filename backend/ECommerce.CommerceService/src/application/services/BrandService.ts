import { IBrandRepository } from '../interfaces/IBrandRepository';
import {
  BrandCreateDto,
  BrandUpdateDto,
  BrandResponseDto
} from '../dtos/BrandDto';
import { Brand } from '../entities/Brand';

export class BrandService {
  constructor(private brandRepository: IBrandRepository) {}

  async getAll(): Promise<BrandResponseDto[]> {
    const brands = await this.brandRepository.findAll();
    return brands.map(this.toResponseDto);
  }

  async getById(id: string): Promise<BrandResponseDto> {
    const brand = await this.brandRepository.findById(id);
    if (!brand) {
      throw new Error('Brand not found');
    }
    return this.toResponseDto(brand);
  }

  async create(dto: BrandCreateDto): Promise<BrandResponseDto> {
    const brand: Brand = {
      ...dto,
      createdAt: new Date(),
      updatedAt: new Date()
    };
    const created = await this.brandRepository.create(brand);
    return this.toResponseDto(created);
  }

  async update(id: string, dto: BrandUpdateDto): Promise<BrandResponseDto> {
    const existing = await this.brandRepository.findById(id);
    if (!existing) {
      throw new Error('Brand not found');
    }
    const updated = await this.brandRepository.update(id, dto);
    if (!updated) {
      throw new Error('Failed to update brand');
    }
    return this.toResponseDto(updated);
  }

  async delete(id: string): Promise<void> {
    const deleted = await this.brandRepository.delete(id);
    if (!deleted) {
      throw new Error('Brand not found');
    }
  }

  private toResponseDto(brand: Brand): BrandResponseDto {
    return {
      _id: brand._id!,
      name: brand.name,
      ...(brand.description && { description: brand.description }),
      ...(brand.logo && { logo: brand.logo }),
      ...(brand.createdAt && { createdAt: brand.createdAt }),
      ...(brand.updatedAt && { updatedAt: brand.updatedAt })
    };
  }
}



