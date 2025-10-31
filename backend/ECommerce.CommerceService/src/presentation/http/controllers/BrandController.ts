import { Request, Response } from 'express';
import { BrandService } from '../../../application/services/BrandService';
import { BrandCreateDto, BrandUpdateDto } from '../../../application/dtos/BrandDto';

export class BrandController {
  constructor(private brandService: BrandService) {}

  getAll = async (req: Request, res: Response): Promise<void> => {
    try {
      const brands = await this.brandService.getAll();
      res.status(200).json(brands);
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  getById = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Brand ID is required' });
        return;
      }
      const brand = await this.brandService.getById(id);
      res.status(200).json(brand);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Brand not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };

  create = async (req: Request, res: Response): Promise<void> => {
    try {
      const dto: BrandCreateDto = req.body;
      const brand = await this.brandService.create(dto);
      res.status(201).json(brand);
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  update = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Brand ID is required' });
        return;
      }
      const dto: BrandUpdateDto = req.body;
      const brand = await this.brandService.update(id, dto);
      res.status(200).json(brand);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Brand not found') {
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
        res.status(400).json({ message: 'Brand ID is required' });
        return;
      }
      await this.brandService.delete(id);
      res.status(204).send();
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Brand not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };
}



