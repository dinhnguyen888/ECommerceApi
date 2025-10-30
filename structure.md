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
