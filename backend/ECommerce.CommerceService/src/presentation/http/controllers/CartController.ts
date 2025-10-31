import { Request, Response } from 'express';
import { CartService } from '../../../application/services/CartService';
import { CartCreateDto, CartUpdateDto } from '../../../application/dtos/CartDto';

export class CartController {
  constructor(private cartService: CartService) {}

  getByUserId = async (req: Request, res: Response): Promise<void> => {
    try {
      const { userId } = req.params;
      if (!userId) {
        res.status(400).json({ message: 'User ID is required' });
        return;
      }
      const cart = await this.cartService.getByUserId(userId);
      if (!cart) {
        res.status(404).json({ message: 'Cart not found' });
      } else {
        res.status(200).json(cart);
      }
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  getById = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Cart ID is required' });
        return;
      }
      const cart = await this.cartService.getById(id);
      res.status(200).json(cart);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Cart not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };

  create = async (req: Request, res: Response): Promise<void> => {
    try {
      const dto: CartCreateDto = req.body;
      const cart = await this.cartService.create(dto);
      res.status(201).json(cart);
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };

  update = async (req: Request, res: Response): Promise<void> => {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Cart ID is required' });
        return;
      }
      const dto: CartUpdateDto = req.body;
      const cart = await this.cartService.update(id, dto);
      res.status(200).json(cart);
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Cart not found') {
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
        res.status(400).json({ message: 'Cart ID is required' });
        return;
      }
      await this.cartService.delete(id);
      res.status(204).send();
    } catch (error) {
      const message = (error as Error).message;
      if (message === 'Cart not found') {
        res.status(404).json({ message });
      } else {
        res.status(500).json({ message });
      }
    }
  };

  deleteByUserId = async (req: Request, res: Response): Promise<void> => {
    try {
      const { userId } = req.params;
      if (!userId) {
        res.status(400).json({ message: 'User ID is required' });
        return;
      }
      await this.cartService.deleteByUserId(userId);
      res.status(204).send();
    } catch (error) {
      res.status(500).json({ message: (error as Error).message });
    }
  };
}



