import { Router } from 'express';
import { ProductController } from '../controllers/ProductController';
import { BrandController } from '../controllers/BrandController';
import { CategoryController } from '../controllers/CategoryController';
import { CartController } from '../controllers/CartController';

export function createRoutes(
  productController: ProductController,
  brandController: BrandController,
  categoryController: CategoryController,
  cartController: CartController
): Router {
  const router = Router();

  // Product routes
  router.get('/products', productController.getAll);
  router.get('/products/:id', productController.getById);
  router.post('/products', productController.create);
  router.put('/products/:id', productController.update);
  router.delete('/products/:id', productController.delete);

  // Brand routes
  router.get('/brands', brandController.getAll);
  router.get('/brands/:id', brandController.getById);
  router.post('/brands', brandController.create);
  router.put('/brands/:id', brandController.update);
  router.delete('/brands/:id', brandController.delete);

  // Category routes
  router.get('/categories', categoryController.getAll);
  router.get('/categories/:id', categoryController.getById);
  router.post('/categories', categoryController.create);
  router.put('/categories/:id', categoryController.update);
  router.delete('/categories/:id', categoryController.delete);

  // Cart routes
  router.get('/carts/user/:userId', cartController.getByUserId);
  router.get('/carts/:id', cartController.getById);
  router.post('/carts', cartController.create);
  router.put('/carts/:id', cartController.update);
  router.delete('/carts/:id', cartController.delete);
  router.delete('/carts/user/:userId', cartController.deleteByUserId);

  return router;
}



