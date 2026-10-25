<div align="center">

# 🛒 E-Commerce RESTful API

**A production-style e-commerce backend built with ASP.NET Core and Onion Architecture: JWT authentication with roles, Redis-backed baskets and response caching, Stripe card payments, and an admin product catalog with image uploads.**

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![EF Core](https://img.shields.io/badge/EF%20Core-10-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Redis](https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=white)](https://redis.io/)
[![Stripe](https://img.shields.io/badge/Stripe-635BFF?logo=stripe&logoColor=white)](https://stripe.com/)
[![JWT](https://img.shields.io/badge/JWT-000000?logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=black)](https://swagger.io/)

[Features](#-features) · [Architecture](#-architecture) · [API Reference](#-api-reference) · [Getting Started](#-getting-started) · [Configuration](#%EF%B8%8F-configuration)

</div>

---

## 📖 Overview

This project is a complete back-end for an online store. It exposes a RESTful API for browsing a product catalog, managing a shopping basket, placing orders, and paying by card with **Stripe**. It was built as part of the **Route Academy** .NET diploma with a focus on **clean architecture**, **SOLID principles**, and **maintainable, testable code**.

## ✨ Features

### 🛍 Catalog
- Product listing with **filtering** (brand, type), **search** by name, **sorting** (name / price, ascending / descending), and **pagination** (default page size 5, capped at 10).
- Brands and product types endpoints for building filter UIs.
- Queries are built with the **Specification pattern**, so filtering, sorting, includes, and paging stay out of the repositories.
- **Redis response caching** via a custom `[RedisCache(seconds)]` action filter. The cache key is built from the request path plus its sorted query string, so each unique filter combination is cached separately.

### 🧺 Basket
- Baskets live entirely in **Redis** (no database round-trips), keyed by a client-generated basket ID, with a **1-day expiry** that refreshes on update.
- Create/update, fetch, and delete endpoints.

### 📦 Orders
- Orders are created from the basket. **Prices are re-read from the database on the server**, so a tampered client-side price can never reach an order.
- Each order line stores a **snapshot** of the product name, picture, and price at purchase time, so history stays accurate when the catalog changes.
- Delivery methods (name, description, delivery time, cost), shipping address stored as an owned entity, and order totals (subtotal + delivery cost).
- Order status tracking (`Pending`, `PaymentReceived`, `PaymentFailed`).
- The basket is cleared automatically once the order is saved.
- Order history lookup by buyer, newest first.

### 💳 Payments (Stripe)
- Creates or **updates a Stripe PaymentIntent** for the current basket and returns the `clientSecret` for the front end to confirm the card payment.
- The amount is **calculated server-side** from database prices plus the chosen delivery cost, then converted to the smallest currency unit.
- Re-calling the endpoint after the basket changes updates the existing intent instead of creating a new one.

### 🔐 Authentication & Authorization
- **ASP.NET Core Identity** with roles (`Admin`, `SuperAdmin`) stored in a dedicated Identity database.
- **JWT Bearer** authentication with validated issuer, audience, lifetime, and signing key; token lifetime is configurable.
- Register, login, email-availability check, current user, and saved shipping address endpoints.
- **Admin-only** product management protected by `[Authorize(Roles = "Admin")]`.

### 🖼 Image Management
- Admins upload product images as `multipart/form-data`.
- Files are stored with **GUID names** (no collisions, no unsafe filenames) and served from `/Files`.
- **Consistency rules**: a new image is deleted if the database save fails, and the old image is deleted only after an update succeeds.

### 🧱 Engineering Practices
- **Result pattern**: services return `Result<T>` with typed errors instead of throwing for expected failures.
- A single base controller maps error types to proper **RFC 7807 `ProblemDetails`** responses (`400`, `401`, `403`, `404`, `409`, `500`).
- Cancellation tokens are propagated through all async layers.
- **Swagger UI** with Bearer authentication support, available in every environment.
- Automatic **migrations and data seeding** on startup.

## 🏗 Architecture

The solution follows **Onion Architecture**. Dependencies point inward, and the domain has no knowledge of infrastructure.

```
┌──────────────────────────────────────────────┐
│  ECommerce.API                               │
│  Controllers · Redis cache filter · Swagger  │
└──────────────────────┬───────────────────────┘
                       │ depends on
┌──────────────────────▼───────────────────────┐
│  ECommerce.Infrastructure                    │
│  EF Core · Redis · Stripe gateway ·          │
│  Image storage · Seeding · Repositories      │
└──────────────────────┬───────────────────────┘
                       │ depends on
┌──────────────────────▼───────────────────────┐
│  ECommerce.Application                       │
│  Services · DTOs · Specifications ·          │
│  AutoMapper profiles · Result pattern        │
└──────────────────────┬───────────────────────┘
                       │ depends on
┌──────────────────────▼───────────────────────┐
│  ECommerce.Domain                            │
│  Entities · Repository & UoW contracts       │
└──────────────────────────────────────────────┘
```

**Patterns & practices**

| Pattern | Where |
|---|---|
| Generic Repository + Unit of Work | `Domain/Contracts`, `Infrastructure/Repositories` |
| Specification | `Application/Specifications` + `SpecificationEvaluator` |
| Result pattern | `Application/Common/Result.cs`, `Error.cs` |
| Dependency Injection | `AddApplicationServices()` / `AddInfrastructureServices()` |
| AutoMapper (with custom value resolvers) | `Application/Profiles` (e.g. building absolute picture URLs) |
| Gateway abstraction | `IPaymentGateway` → `StripePaymentGateway` (the payment provider is swappable) |
| Keyed services | `Catalog` and `Identity` data seeders |
| Action filter | `RedisCacheAttribute` |
| Separate databases | Store data and Identity data live in two different databases |

## 🛠 Tech Stack

| Area | Technology |
|---|---|
| Framework | ASP.NET Core Web API on **.NET 10** |
| Data | Entity Framework Core 10, SQL Server (Store DB + Identity DB) |
| Identity | ASP.NET Core Identity, JWT Bearer |
| Cache & baskets | Redis (StackExchange.Redis) |
| Payments | Stripe.net (PaymentIntents) |
| Mapping | AutoMapper |
| Docs | Swagger / Swashbuckle, OpenAPI |

## 📡 API Reference

🔒 = requires `Authorization: Bearer <token>`.

### Authentication · `api/Authentication`
| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/login` | Log in and receive a JWT |
| `POST` | `/Register` | Create an account and receive a JWT |
| `GET` | `/EmailExists?email=` | Check whether an email is already registered |
| `GET` | `/CurrentUser?email=` 🔒 | Get the current user |
| `GET` | `/Address?email=` 🔒 | Get the saved shipping address |
| `POST` | `/Address?email=` 🔒 | Create or update the saved shipping address |

### Products · `api/Product`
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/` 🔒 | List products (cached). Query: `brandId`, `typeId`, `searchValue`, `sort`, `PageIndex`, `PageSize` |
| `GET` | `/{id}` | Get a product by ID |
| `GET` | `/Brands` | List brands |
| `GET` | `/Types` | List product types |
| `POST` | `/Admin` 🔒 *(Admin)* | Create a product (multipart form with image) |
| `PUT` | `/{id}/Admin` 🔒 *(Admin)* | Update a product; only the fields you send change |
| `DELETE` | `/{id}/Admin` 🔒 *(Admin)* | Delete a product and its image |

`sort` values: `NameAsc`, `NameDesc`, `PriceAsc`, `PriceDesc`.

### Basket · `api/Basket`
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/{id}` | Get a basket |
| `POST` | `/` | Create or update a basket |
| `DELETE` | `/{id}` | Delete a basket |

### Orders · `api/Order`
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/DeliveryMethods` | List delivery methods |
| `POST` | `/?email=` 🔒 | Create an order from a basket |
| `GET` | `/?email=` 🔒 | List a buyer's orders (newest first) |
| `GET` | `/{id}?email=` 🔒 | Get one order |

### Payments · `api/Payments`
| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/?basketId=` 🔒 | Create or update the Stripe PaymentIntent for a basket; returns the basket with `clientSecret` |

## 🔄 Typical Checkout Flow

```
1. POST /api/Authentication/Register     → get JWT
2. GET  /api/Product                     → browse (filter, search, sort, page)
3. POST /api/Basket                      → save basket (Redis)
4. GET  /api/Order/DeliveryMethods       → choose delivery, set deliveryMethodId on the basket
5. POST /api/Payments?basketId=...       → get Stripe clientSecret, confirm payment on the client
6. POST /api/Order?email=...             → place the order (basket is cleared)
7. GET  /api/Order?email=...             → view order history
```

## 📁 Project Structure

```
ECommerceAPI/
├── ECommerce.Domain/
│   ├── Common/              # BaseEntity<TKey>
│   ├── Contracts/           # Repository, UnitOfWork, Specification, Seeder interfaces
│   └── Entities/            # Products, Orders, Baskets, Identity
├── ECommerce.Application/
│   ├── Common/              # Result, Error, PaginatedResult
│   ├── Contracts/           # Service interfaces
│   ├── DTO's/               # Basket, Identity, Order, Products
│   ├── Params/              # ProductQueryParams, sorting options
│   ├── Profiles/            # AutoMapper profiles + picture URL resolvers
│   ├── Services/            # Auth, Identity, Token, Product, Basket, Order, Payment, Cache
│   └── Specifications/      # Product & Order query specifications
├── ECommerce.Infrastructure/
│   ├── Data/                # StoreDbContext, StoreIdentityDbContext, configurations
│   ├── Migrations/
│   ├── Payments/            # StripePaymentGateway
│   ├── Repositories/        # Generic repo, UnitOfWork, Basket (Redis), Cache (Redis)
│   ├── Seeding/             # Catalog + Identity seeders
│   └── Services/            # ImageService
└── ECommerce.API/
    ├── Attribute/           # RedisCacheAttribute
    ├── Controllers/
    ├── Extentions/          # Migration + seed on startup
    ├── Files/Images/        # Product images (served at /Files)
    └── Program.cs
```

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full)
- [Redis](https://redis.io/download), for example via Docker:
  ```bash
  docker run -d --name redis -p 6379:6379 redis
  ```
- A [Stripe](https://stripe.com) account in **test mode** (secret key)

### Run locally

```bash
# 1. Clone
git clone https://github.com/Islam3mar/ECommerceAPI.git
cd ECommerceAPI/ECommerce.API

# 2. Configure secrets (never commit these)
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:StoreDbConnection"    "Server=.;Database=ECommerceStore;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet user-secrets set "ConnectionStrings:IdentityConnection"   "Server=.;Database=ECommerceIdentity;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet user-secrets set "ConnectionStrings:RedisConnection"      "localhost"
dotnet user-secrets set "JWT:SecretKey"  "<a-long-random-string-at-least-32-characters>"
dotnet user-secrets set "JWT:Issuer"     "https://localhost:7269/"
dotnet user-secrets set "JWT:Audience"   "https://localhost:7269/"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."

# 3. Run (migrations and seeding are applied automatically)
dotnet run
```

Open **Swagger UI** at `https://localhost:7269/swagger`. To call protected endpoints, log in, copy the `token` from the response, click **Authorize**, and paste the token only.

On first run the seeders create the store tables, load the sample catalog (brands, types, products, and delivery methods) when the tables are empty, create the `Admin` and `SuperAdmin` roles, and add an initial admin user. Change that user's credentials in `IdentityDataSeeder` (or move them to configuration) before deploying anywhere public.

## ⚙️ Configuration

| Key | Description |
|---|---|
| `ConnectionStrings:StoreDbConnection` | SQL Server connection string for catalog and orders |
| `ConnectionStrings:IdentityConnection` | SQL Server connection string for users and roles |
| `ConnectionStrings:RedisConnection` | Redis connection string (baskets + response cache) |
| `UrlSettings:BaseUrl` | Public base URL, used to build absolute product image URLs |
| `JWT:SecretKey` | Signing key for tokens. Use a long, random value |
| `JWT:Issuer` / `JWT:Audience` | Token issuer and audience, validated on every request |
| `JWT:ExpirationInMinutes` | Token lifetime (default `60`) |
| `Stripe:SecretKey` | Stripe secret API key |
| `Stripe:DefaultCurrency` | Payment currency (default `USD`) |

> In production, supply secrets through environment variables or your host's secret store, never through files in the repository.

## 🔒 Security

- JWT validation checks **issuer, audience, expiry, and signing key**.
- Passwords are handled by **ASP.NET Core Identity** (hashed and salted).
- Admin operations are restricted by **role-based authorization**.
- Order totals and payment amounts are **always calculated on the server** from database prices.
- Uploaded files get **random GUID names**, never the client's filename.
- The Stripe secret key is read from configuration and is never exposed to clients; only the PaymentIntent `clientSecret` is returned.

## 🗺 Roadmap

- [ ] **Stripe webhook** to move orders from `Pending` to `PaymentReceived` / `PaymentFailed` automatically.
- [ ] **Cache invalidation** when an admin creates, updates, or deletes a product.
- [ ] Automated **unit and integration tests**.

## 👤 Author

**Islam Omar**, Back-End Developer (.NET)

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0A66C2?logo=linkedin&logoColor=white)](https://linkedin.com/in/islam-omar-3ba867266)
[![GitHub](https://img.shields.io/badge/GitHub-181717?logo=github&logoColor=white)](https://github.com/Islam3mar)
[![Portfolio](https://img.shields.io/badge/Portfolio-1F5F7A?logoColor=white)](https://islam3mar.github.io/portfolio/)
[![Email](https://img.shields.io/badge/Email-D14836?logo=gmail&logoColor=white)](mailto:islam3marahmed@gmail.com)

Built as part of the **Back-End Development Diploma (.NET Track)** at **Route Academy**.
