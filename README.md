## 🪐 **Mercury Platform**

A distributed event-driven backend microservices platform built with .NET 8, implementing Clean Architecture, Domain-Driven Design (DDD), and CQRS patterns.

## 🏗️ Architecture

Mercury Platform is a microservices-based e-commerce backend system consisting of four main services:

- **Orders API**: Manages order creation, processing, and tracking (PostgreSQL)
- **Inventory API**: Handles product inventory and stock management (MongoDB)
- **Payments API**: Processes payments and transactions (PostgreSQL)
- **Checkout Orchestrator**: Coordinates the checkout process across services (MongoDB)

### Technology Stack

- **.NET 8**: Latest LTS version of .NET
- **Clean Architecture**: Separation of concerns with Domain, Application, Infrastructure, and API layers
- **DDD (Domain-Driven Design)**: Rich domain models with business logic
- **CQRS**: Command Query Responsibility Segregation using MediatR
- **PostgreSQL**: Relational database for Orders and Payments
- **MongoDB**: Document database for Inventory and Checkout
- **Redis**: Distributed caching
- **RabbitMQ**: Message broker for event-driven communication
- **Docker**: Containerization for all services
- **AutoMapper**: Object-to-object mapping
- **Serilog**: Structured logging
- **xUnit**: Unit testing framework

## 📁 Project Structure

```
MercuryPlatform/
├── src/
│   ├── Services/
│   │   ├── Orders/
│   │   │   ├── Orders.API/           # REST API endpoints
│   │   │   ├── Orders.Application/   # CQRS handlers, DTOs
│   │   │   ├── Orders.Domain/        # Domain entities, value objects
│   │   │   └── Orders.Infrastructure/ # EF Core, repositories
│   │   ├── Inventory/
│   │   │   ├── Inventory.API/
│   │   │   ├── Inventory.Application/
│   │   │   ├── Inventory.Domain/
│   │   │   └── Inventory.Infrastructure/
│   │   ├── Payments/
│   │   │   ├── Payments.API/
│   │   │   ├── Payments.Application/
│   │   │   ├── Payments.Domain/
│   │   │   └── Payments.Infrastructure/
│   │   └── Checkout/
│   │       ├── Checkout.API/
│   │       ├── Checkout.Application/
│   │       ├── Checkout.Domain/
│   │       └── Checkout.Infrastructure/
│   └── Shared/
│       ├── Shared.Contracts/         # Event contracts, interfaces
│       └── Shared.Infrastructure/    # Common infrastructure code
├── tests/
│   ├── Orders.UnitTests/
│   ├── Inventory.UnitTests/
│   ├── Payments.UnitTests/
│   └── Checkout.UnitTests/
├── docker-compose.yml
└── MercuryPlatform.sln
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/downloads)

### Running with Docker Compose

1. Clone the repository:
```bash
git clone https://github.com/EsdrasRR/Mercury-Platform.git
cd Mercury-Platform
```

2. Build and run all services:
```bash
docker-compose up -d
```

3. Access the services:
- Orders API: http://localhost:5001
- Inventory API: http://localhost:5002
- Payments API: http://localhost:5003
- Checkout API: http://localhost:5004
- RabbitMQ Management: http://localhost:15672 (user: mercury, pass: mercury123)

### Running Locally

1. Start infrastructure services:
```bash
docker-compose up -d postgres mongodb redis rabbitmq
```

2. Build the solution:
```bash
dotnet build MercuryPlatform.sln
```

3. Run tests:
```bash
dotnet test MercuryPlatform.sln
```

4. Run individual services:
```bash
# Orders API
cd src/Services/Orders/Orders.API
dotnet run

# Inventory API
cd src/Services/Inventory/Inventory.API
dotnet run

# Payments API
cd src/Services/Payments/Payments.API
dotnet run

# Checkout API
cd src/Services/Checkout/Checkout.API
dotnet run
```

## 📊 Service Ports

| Service | Port |
|---------|------|
| Orders API | 5001 |
| Inventory API | 5002 |
| Payments API | 5003 |
| Checkout API | 5004 |
| PostgreSQL | 5432 |
| MongoDB | 27017 |
| Redis | 6379 |
| RabbitMQ | 5672 |
| RabbitMQ Management | 15672 |

## 🏛️ Clean Architecture Layers

### Domain Layer
- Contains enterprise business rules
- Entities with business logic
- Value objects
- Domain events
- No dependencies on other layers

### Application Layer
- Contains application business rules
- CQRS Commands and Queries
- Command/Query Handlers (MediatR)
- DTOs and AutoMapper profiles
- Interfaces for infrastructure
- Depends only on Domain layer

### Infrastructure Layer
- Contains infrastructure concerns
- Database contexts (EF Core, MongoDB)
- Repository implementations
- Message broker integrations
- External service clients
- Depends on Application and Domain layers

### API Layer
- Contains API endpoints
- Dependency injection configuration
- Middleware pipeline
- Serilog configuration
- Depends on Application and Infrastructure layers

## 🔄 CQRS Pattern

Each service implements CQRS using MediatR:

```csharp
// Command
public class CreateOrderCommand : IRequest<OrderDto>
{
    public string CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

// Command Handler
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Business logic here
    }
}
```

## 📨 Event-Driven Communication

Services communicate asynchronously using RabbitMQ:

- **OrderCreated**: Published when an order is created
- **InventoryReserved**: Published when inventory is reserved
- **PaymentProcessed**: Published when payment is processed
- **CheckoutCompleted**: Published when checkout is completed

## 🧪 Testing

Run all tests:
```bash
dotnet test MercuryPlatform.sln
```

Run tests with coverage:
```bash
dotnet test MercuryPlatform.sln --collect:"XPlat Code Coverage"
```

## 📝 Logging

All services use Serilog for structured logging:
- Console sink for development
- File sink for production
- Enriched with context information

## 🔧 Configuration

Each service can be configured via `appsettings.json` or environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=mercury_orders;Username=mercury;Password=mercury123"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "mercury",
    "Password": "mercury123"
  },
  "Redis": {
    "Configuration": "localhost:6379"
  }
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Authors

- **EsdrasRR** - [GitHub Profile](https://github.com/EsdrasRR)

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- CQRS pattern by Greg Young
