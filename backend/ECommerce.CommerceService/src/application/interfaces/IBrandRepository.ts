import { Brand } from '../entities/Brand';

export interface IBrandRepository {
  findAll(): Promise<Brand[]>;
  findById(id: string): Promise<Brand | null>;
  create(brand: Brand): Promise<Brand>;
  update(id: string, brand: Partial<Brand>): Promise<Brand | null>;
  delete(id: string): Promise<boolean>;
}

