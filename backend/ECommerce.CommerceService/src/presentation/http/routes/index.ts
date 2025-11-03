import { Router } from 'express';
import { ProductController } from '../controllers/ProductController';
import { BrandController } from '../controllers/BrandController';
import { CategoryController } from '../controllers/CategoryController';
import { CartController } from '../controllers/CartController';
import { authenticate, authenticateAndAuthorize } from '../middlewares/authMiddleware';

export function createRoutes(
  productController: ProductController,
  brandController: BrandController,
  categoryController: CategoryController,
  cartController: CartController
): Router {
  const router = Router();

  // Routes cho san pham
  // GET endpoints - cong khai (co the xem)
  router.get('/products', productController.getAll);
  router.get('/products/:id', productController.getById);
  
  // POST, PUT, DELETE - yeu cau role Admin
  router.post('/products', ...authenticateAndAuthorize('Admin'), productController.create);
  router.put('/products/:id', ...authenticateAndAuthorize('Admin'), productController.update);
  router.delete('/products/:id', ...authenticateAndAuthorize('Admin'), productController.delete);

  // Routes cho thuong hieu
  // GET endpoints - cong khai
  router.get('/brands', brandController.getAll);
  router.get('/brands/:id', brandController.getById);
  
  // POST, PUT, DELETE - yeu cau role Admin
  router.post('/brands', ...authenticateAndAuthorize('Admin'), brandController.create);
  router.put('/brands/:id', ...authenticateAndAuthorize('Admin'), brandController.update);
  router.delete('/brands/:id', ...authenticateAndAuthorize('Admin'), brandController.delete);

  // Routes cho danh muc
  // GET endpoints - cong khai
  router.get('/categories', categoryController.getAll);
  router.get('/categories/:id', categoryController.getById);
  
  // POST, PUT, DELETE - yeu cau role Admin
  router.post('/categories', ...authenticateAndAuthorize('Admin'), categoryController.create);
  router.put('/categories/:id', ...authenticateAndAuthorize('Admin'), categoryController.update);
  router.delete('/categories/:id', ...authenticateAndAuthorize('Admin'), categoryController.delete);

  // Routes cho gio hang
  // Tat ca cac thao tac gio hang yeu cau xac thuc (Client hoac Admin)
  router.get('/carts/user/:userId', authenticate, cartController.getByUserId);
  router.get('/carts/:id', authenticate, cartController.getById);
  router.post('/carts', authenticate, cartController.create);
  router.put('/carts/:id', authenticate, cartController.update);
  router.delete('/carts/:id', authenticate, cartController.delete);
  router.delete('/carts/user/:userId', authenticate, cartController.deleteByUserId);

  return router;
}



