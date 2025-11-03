import express, { Express } from 'express';
import cors from 'cors';
import { connectDatabase } from './infrastructure/config/database';
import { RabbitMqConnection } from './infrastructure/messageBroker/rabbitmqConnection';
import { ProductRepository } from './infrastructure/repositories/ProductRepository';
import { BrandRepository } from './infrastructure/repositories/BrandRepository';
import { CategoryRepository } from './infrastructure/repositories/CategoryRepository';
import { CartRepository } from './infrastructure/repositories/CartRepository';
import { ProductService } from './application/services/ProductService';
import { BrandService } from './application/services/BrandService';
import { CategoryService } from './application/services/CategoryService';
import { CartService } from './application/services/CartService';
import { ProductController } from './presentation/http/controllers/ProductController';
import { BrandController } from './presentation/http/controllers/BrandController';
import { CategoryController } from './presentation/http/controllers/CategoryController';
import { CartController } from './presentation/http/controllers/CartController';
import { createRoutes } from './presentation/http/routes/index';

export async function createApp(): Promise<Express> {
  await connectDatabase();

  const rabbitMqConnection = new RabbitMqConnection();
  await rabbitMqConnection.connect();

  // Khoi tao repositories
  const productRepository = new ProductRepository();
  const brandRepository = new BrandRepository();
  const categoryRepository = new CategoryRepository();
  const cartRepository = new CartRepository();

  // Khoi tao services
  const productService = new ProductService(productRepository);
  const brandService = new BrandService(brandRepository);
  const categoryService = new CategoryService(categoryRepository);
  const cartService = new CartService(cartRepository);

  // Khoi tao controllers
  const productController = new ProductController(productService);
  const brandController = new BrandController(brandService);
  const categoryController = new CategoryController(categoryService);
  const cartController = new CartController(cartService);

  // Tao Express app
  const app = express();

  // Middlewares
  app.use(cors());
  app.use(express.json());
  app.use(express.urlencoded({ extended: true }));

  // Routes
  app.use('/api', createRoutes(
    productController,
    brandController,
    categoryController,
    cartController
  ));

  // Health check
  app.get('/health', (req: express.Request, res: express.Response) => {
    res.status(200).json({ status: 'OK', service: 'CommerceService' });
  });

  return app;
}

