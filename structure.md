.NET Clean
ECommerce_Clean/
│
├── Application/
│   ├── Common/
│   │   ├── Behaviors/          # Validation, Logging, Caching pipeline behaviors
│   │   ├── Exceptions/         # Custom exceptions
│   │   ├── Mappings/           # AutoMapper profiles
│   │   └── Utils/              # Helper classes, extensions
│   │
│   ├── Dtos/                   # DTOs for requests/responses
│   ├── Entities/               # Domain models (Product, Order, etc.)
│   ├── Interfaces/             # Interfaces for Repo, Services, MQ, Cache, etc.
│   └── Services/               # Business logic implementations
│
├── Infrastructure/
│   ├── Configurations/         # appsettings, JWT, dependency injection
│   ├── Persistence/            # DbContext, EF Config, Migrations
│   ├── Repositories/           # Repo implementations (implements Application.Interfaces)
│   └── MessageBroker/          # RabbitMQ publishers/subscribers
│
├── Presentation/
│   ├── Http/
│   │   ├── Controllers/        # API endpoints
│   │   ├── Filters/            # Exception filters, authorization filters
│   │   └── Middlewares/        # Custom middlewares (JWT, logging, etc.)
│   │
│   ├── WS/                     # WebSocket / SignalR endpoints
│   └── Background/             # Background services, MQ consumers
│
└── Program.cs                  # Entry point (.NET 8 Minimal API / Startup)


# NodeJS Clean Architecture version (for reference)

ECommerce_Clean/
│
├── src/
│   ├── application/                # (Tầng use case / business logic)
│   │   ├── common/
│   │   │   ├── behaviors/          # Middleware-like logic (validation, logging, caching)
│   │   │   ├── exceptions/         # Custom error classes
│   │   │   ├── mappings/           # Mapping / transformation logic
│   │   │   └── utils/              # Helper functions, formatters
│   │
│   │   ├── dtos/                   # Data Transfer Objects (request/response schemas)
│   │   ├── entities/               # Domain models (Product, Order, etc.)
│   │   ├── interfaces/             # Abstract contracts (repositories, cache, mq)
│   │   └── services/               # Business logic (use cases)
│
├── infrastructure/                 # (Tầng giao tiếp hạ tầng)
│   ├── config/                     # Configs (env, JWT, DB, DI container)
│   ├── persistence/                # Database setup (Prisma / Sequelize / Mongoose)
│   ├── repositories/               # Implement repositories
│   └── message-broker/             # RabbitMQ / Kafka producer & consumer
│
├── presentation/                   # (Tầng entry point - giống Controller trong .NET)
│   ├── http/
│   │   ├── controllers/            # Express route handlers (controller functions)
│   │   ├── middlewares/            # Custom middlewares (auth, error, logging)
│   │   └── validators/             # Request validation (Joi/Zod/Yup)
│   │
│   ├── ws/                         # Socket.IO / WebSocket gateway
│   └── background/                 # Background jobs (BullMQ / cron / consumer)
│
├── app.js                          # Express app setup (register middlewares, routes)
├── server.js                       # Entry point (start server)
└── package.json


users [icon: user, color: blue] {
  id string pk
  userName string
  password string
  role enum(admin, client)
  email string
  createdAt DateTime
  updatedAt DateTime
  isActive bool
}

refreshTokens [icon: key-round, color: blue] {
  id string pk
  userId string
  token string
  expiredAt DateTime
}

products [icon: gcp-producer-portal, color: green] {
  id string pk
  name string
  description string
  price decimal
  createdAt DateTime
  fileUrl string
  fileType string
}



orders [icon: shopping-cart, color: orange] {
  id string pk
  userId string fk
  totalAmount decimal
  status enum(pending, paid, failed)
  createdAt DateTime
}

orderItems [icon: package, color: orange] {
  id string pk
  orderId string fk
  productId string fk
  price decimal
}

payments [icon: dollar-sign, color: orange] {
  id string pk
  orderId string fk
  paymentMethod enum(card, paypal, momo)
  transactionId string
  paidAt DateTime
  amount decimal
}

notifications [icon: bell, color: purple] {
  id string pk
  userId string fk allow null 
  title string
  message string
  sentAt DateTime
  type enum(email, system, personal)
}

emailLogs [icon: mail, color: purple] {
  id string pk
  notificationId string fk allow null
  email string
  sentAt DateTime
  status enum(success, failed)
}


# Relationships
refreshTokens.userId > users.id
orderItems.orderId > orders.id
orderItems.productId > products.id
orders.userId > users.id
payments.orderId > orders.id
notifications.userId > users.id
emailLogs.notificationId > notifications.id
