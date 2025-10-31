# ECommerce CommerceService

CommerceService microservice for managing Products, Brands, Categories, and Carts.

## Architecture

This service follows Clean Architecture principles with the following structure:

```
src/
├── application/          # Business logic layer
│   ├── entities/         # Domain models
│   ├── dtos/            # Data Transfer Objects
│   ├── interfaces/      # Repository interfaces
│   └── services/        # Business logic services
├── infrastructure/       # Infrastructure layer
│   ├── persistence/     # MongoDB models
│   ├── repositories/    # Repository implementations
│   └── config/          # Configuration (database, etc.)
└── presentation/         # Presentation layer
    └── http/            # HTTP controllers and routes
```

## Setup

1. Install dependencies:
```bash
npm install
```

2. Create `.env` file:
```
PORT=3001
MONGODB_URI=mongodb://localhost:27017/ecommerce
```

3. Build TypeScript:
```bash
npm run build
```

4. Start server:
```bash
npm start
```

For development with auto-reload:
```bash
npm run dev
```

## API Endpoints

### Products
- `GET /api/products` - Get all products
- `GET /api/products/:id` - Get product by ID
- `POST /api/products` - Create product
- `PUT /api/products/:id` - Update product
- `DELETE /api/products/:id` - Delete product

### Brands
- `GET /api/brands` - Get all brands
- `GET /api/brands/:id` - Get brand by ID
- `POST /api/brands` - Create brand
- `PUT /api/brands/:id` - Update brand
- `DELETE /api/brands/:id` - Delete brand

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/:id` - Get category by ID
- `POST /api/categories` - Create category
- `PUT /api/categories/:id` - Update category
- `DELETE /api/categories/:id` - Delete category

### Carts
- `GET /api/carts/user/:userId` - Get cart by user ID
- `GET /api/carts/:id` - Get cart by ID
- `POST /api/carts` - Create cart
- `PUT /api/carts/:id` - Update cart
- `DELETE /api/carts/:id` - Delete cart
- `DELETE /api/carts/user/:userId` - Delete cart by user ID

## Health Check

- `GET /health` - Service health check

