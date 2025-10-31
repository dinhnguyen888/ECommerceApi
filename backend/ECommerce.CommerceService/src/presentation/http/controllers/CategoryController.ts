import { Request, Response } from 'express';
import { CategoryService } from '../../../application/services/CategoryService';
import { CategoryCreateDto, CategoryUpdateDto } from '../../../application/dtos/CategoryDto';

export class CategoryController {
  constructor(private categoryService: CategoryService) {}

  getAll = async (req: Request, res: Response): Promise<void> => {
    try {
      const categories = await this.categoryService.getAll();
      res.status(200).json(categories);
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  getById = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Category ID is required' });
        return;
      }
      const category = await this.categoryService.getById(id);
      res.status(200).json(category);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Category not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };

  create = async (req: Request, res: Response): Promise<void> => {
    try {
      const dto: CategoryCreateDto = req.body;
      const category = await this.categoryService.create(dto);
      res.status(201).json(category);
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  update = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Category ID is required' });
        return;
      }
      const dto: CategoryUpdateDto = req.body;
      const category = await this.categoryService.update(id, dto);
      res.status(200).json(category);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Category not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };

  delete = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Category ID is required' });
        return;
      }
      await this.categoryService.delete(id);
      res.status(204).send();
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Category not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };
}



