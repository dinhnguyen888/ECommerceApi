import { Request, Response } from 'express';
import { ProductService } from '../../../application/services/ProductService';
import { ProductCreateDto, ProductUpdateDto } from '../../../application/dtos/ProductDto';

export class ProductController {
  constructor(private productService: ProductService) {}

  getAll = async (req: Request, res: Response): Promise<void> => {
    try {
      const products = await this.productService.getAll();
      res.status(200).json(products);
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  getById = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Product ID is required' });
        return;
      }
      const product = await this.productService.getById(id);
      res.status(200).json(product);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Product not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };

  create = async (req: Request, res: Response): Promise<void> => {
    try {
      const dto: ProductCreateDto = req.body;
      const product = await this.productService.create(dto);
      res.status(201).json(product);
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  update = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Product ID is required' });
        return;
      }
      const dto: ProductUpdateDto = req.body;
      const product = await this.productService.update(id, dto);
      res.status(200).json(product);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Product not found') {
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
        res.status(400).json({ message: 'Product ID is required' });
        return;
      }
      await this.productService.delete(id);
      res.status(204).send();
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Product not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };
}



