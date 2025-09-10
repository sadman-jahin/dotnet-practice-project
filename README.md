# 🧾 Inventory App

An enterprise-grade inventory management application designed with scalability, maintainability, and performance in mind. This application leverages modern architectural patterns and asynchronous event-driven communication to provide a robust inventory and order management system.

---

## 🚀 Tech Stack

- **.NET Core 9** – Backend API
- **RabbitMQ** – Event-driven messaging
- **Hangfire** – Background job scheduling
- **Clean Architecture** – Layered and modular design for better maintainability and testing
- **Polly** – Resilience and transient-fault-handling for inter-service communication
- **Entity Framework Core** – ORM for data access
---

## 🏗️ Architectural Overview

### ✅ Modular Approach with Clean Architecture

The project is structured using **Clean Architecture** principles, ensuring clear separation of concerns and modular development.

- **Domain Layer**: Contains core business logic and entities, independent of any frameworks or external dependencies.
- **Application Layer**: Defines business use cases, interfaces, and DTOs. This layer orchestrates the domain logic.
- **Infrastructure Layer**: Contains implementations for database access, external services (e.g., email, RabbitMQ), and third-party integrations.
- **Presentation Layer**: ASP.NET Core Web API that acts as the entry point for external clients and services.

Benefits:
- Easier to maintain and test
- Decoupled from infrastructure concerns
- Plug-and-play modules for adding new features or services

---

## 📨 Event-Driven Mail Sending with RabbitMQ

Email notifications (e.g., order confirmation, inventory updates, etc.) are handled asynchronously using **RabbitMQ**, following an **event-driven architecture**.

### Flow:

1. Business events (e.g., `Order Closed`) are published to RabbitMQ.
2. A background consumer listens to these queues and triggers the appropriate email services.
3. This allows the main application flow to remain fast and responsive while ensuring eventual consistency for non-critical operations.

Advantages:
- Improved application performance (no blocking on email I/O)
- Loose coupling between services
- Easier to scale email handling independently

---

## 🔄 Order Closing Tool with Hangfire

A separate micro-application/service is responsible for **closing pending orders**, implemented using **Hangfire** for recurring job scheduling.

### Key Features:

- Independent .NET Core 9 application
- Periodically checks and closes stale or pending orders
- Runs scheduled jobs using Hangfire (e.g., every 30 minutes or 1 hour)
- Can be hosted as a Windows/Linux service or deployed in the cloud (e.g., Azure WebJobs, AWS Lambda)

### Benefits:

- Keeps the main application lean and focused
- Enables distributed background processing
- Improves order system hygiene and customer experience

---

## 📦 Features

- CRUD operations for inventory and products
- Order placement and management
- Asynchronous event-based email notifications
- Scheduled cleanup and order finalization jobs
- Modular, extensible codebase with dependency injection and interface segregation

---

## 🧪 Testing (To be implemented)

- Unit testing at domain and application layers
- Integration testing with in-memory databases and mocked RabbitMQ
- End-to-end testing for key workflows

---

## 📁 Folder Structure (Example)

```plaintext
InventoryModule/
│
├── Inventory.Presentation/        --> ASP.NET Core Web API
├── Inventory.Application/         --> Application layer (use cases, services, DTOs)
├── Inventory.Domain/              --> Domain layer (entities, value objects)
├── Inventory.Infrastructure/      --> Infrastructure layer (DB)

OrderModule/
│
├── Orders.Presentation/           --> ASP.NET Core Web API
├── Orders.Application/            --> Application layer (use cases, services, DTOs)
├── Orders.Domain/                 --> Domain layer (entities, value objects)
├── Orders.Infrastructure/         --> Infrastructure layer (DB)

Messaging/
│
├── Messaging.Application/         --> RabbitMQ Integration 

MailSender/
│
├── MailSender.Application/        --> MailSender App

ApiClient/
│
├──  ApiClient.Application/        --> Global Api Client Handler for Api Requests

OrderClosingTool/
│
├── OrderClosingTool.Application/  --> Separate app for scheduled order closing (Hangfire)

