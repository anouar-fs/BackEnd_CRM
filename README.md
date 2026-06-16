# BackEnd CRM

A robust RESTful API backend for a CRM (Customer Relationship Management) system, built with **ASP.NET Core 8** and **C#**. It handles leads, events, advisors, and user authentication, with full-text search powered by Typesense.

---

## 🚀 Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 (.NET 8) |
| Language | C# |
| ORM | Entity Framework Core 8 |
| Database | MySQL |
| Authentication | JWT Bearer Tokens |
| Validation | FluentValidation |
| Full-text Search | Typesense |
| Serialization | Newtonsoft.Json |
| API Docs | Swagger / Swashbuckle |
| Containerization | Docker |

---

## 📁 Project Structure

```
BackEnd_CRM/
├── .github/workflows/        # CI/CD pipelines
├── BddContext/               # Database context (AppDbContext)
├── Configuration/            # App configuration classes (JWT, Typesense settings)
├── Controller/               # API controllers (Auth, Lead, Event, Advisor)
├── Dto/                      # Data Transfer Objects
├── Entities/                 # EF Core entity models
├── Mapper/                   # Object mappers (Lead, Event, Advisor)
├── Migrations/               # EF Core database migrations
├── Models/                   # Additional models / response wrappers
├── Properties/               # Launch settings
├── Repositories/             # Data access layer (interfaces + implementations)
│   ├── Advisor/
│   ├── Event/
│   └── Lead/
├── Services/                 # Business logic layer
│   ├── Advisor/
│   ├── Event/
│   └── Lead/
├── Validation/               # FluentValidation validators
├── Program.cs                # App entry point and DI configuration
├── appsettings.json          # App configuration
├── Dockerfile                # Docker build instructions
└── BackEnd.csproj            # Project file
```

---

## ✨ Features

- **JWT Authentication** — Secure token-based auth with configurable issuer, audience, and expiry
- **Global Authorization** — All endpoints require authentication by default via a global `AuthorizeFilter`
- **Lead Management** — Full CRUD for CRM leads
- **Event Management** — Create, read, update, delete events
- **Advisor Management** — Manage advisor profiles and assignments
- **Full-text Search** — Integrated with [Typesense](https://typesense.org/) for fast, typo-tolerant search
- **Validation** — Request validation using FluentValidation with auto-validation middleware
- **CORS** — Configured for local frontend (`http://localhost:5173`) and ngrok tunnels
- **Swagger UI** — Interactive API documentation with Bearer token support
- **Docker Support** — Multi-stage Dockerfile for optimized production images
- **Repository Pattern** — Clean separation between data access and business logic

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [MySQL](https://dev.mysql.com/downloads/) (default port `3306`)
- [Typesense](https://typesense.org/docs/guide/install-typesense.html) (default port `8108`)
- [Docker](https://www.docker.com/) *(optional)*

### 1. Clone the repository

```bash
git clone https://github.com/anouar-fs/BackEnd_CRM.git
cd BackEnd_CRM
```

### 2. Configure the application

The `appsettings.json` file contains **no secrets** — all sensitive values are injected via environment variables or .NET User Secrets.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Jwt": {
    "Key": "",
    "Issuer": "Automatapp",
    "Audience": "Automat_users",
    "ExpireMinutes": 60
  },
  "Typesense": {
    "ApiKey": "",
    "Host": "localhost",
    "Port": "8108",
    "Protocol": "http"
  }
}
```

### Provide secrets for local development (User Secrets)

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "your_super_secret_key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=AutomApp;User=root;Password=yourpassword;"
dotnet user-secrets set "Typesense:ApiKey" "your_typesense_api_key"
```

> Secrets are stored outside the project folder and are never committed to Git.

### Provide secrets for production (Environment Variables)

```bash
export Jwt__Key="your_super_secret_key"
export ConnectionStrings__DefaultConnection="Server=...;Password=prod_password;"
export Typesense__ApiKey="your_typesense_api_key"
```

Or with Docker:
```bash
docker run -p 8080:8080 \
  -e Jwt__Key="your_super_secret_key" \
  -e ConnectionStrings__DefaultConnection="Server=db;Password=prod;" \
  -e Typesense__ApiKey="your_typesense_api_key" \
  backend-crm
```

### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).  
Swagger UI: `http://localhost:5000/swagger`

---

## 🐳 Docker

### Build and run with Docker

```bash
# Build the image
docker build -t backend-crm .

# Run the container
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Port=3306;Database=AutomApp;User=root;Password=yourpassword;" \
  -e Jwt__Key="YOUR_SUPER_SECRET_KEY" \
  backend-crm
```

---

## 🔐 Authentication

All endpoints are protected by default. To access them:

1. Call the `/api/auth/login` endpoint with valid credentials to receive a JWT token.
2. Include the token in the `Authorization` header of subsequent requests:

```
Authorization: Bearer <your-token>
```

The Swagger UI also supports the Bearer token input directly through the **Authorize** button.

---

## 🧩 NuGet Dependencies

| Package | Version |
|---|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 8.0.0 |
| `Microsoft.EntityFrameworkCore` | 8.0.20 |
| `MySql.EntityFrameworkCore` | 9.0.9 |
| `FluentValidation.AspNetCore` | 11.3.1 |
| `Swashbuckle.AspNetCore` | 8.0.0 |
| `Typesense` | 8.3.0 |
| `Microsoft.AspNetCore.Mvc.NewtonsoftJson` | 8.0.0 |

---

## 🗂️ API Overview

| Module | Base Route | Description |
|---|---|---|
| Auth | `/api/auth` | Register and login, returns JWT |
| Leads | `/api/leads` | CRUD for CRM leads |
| Events | `/api/events` | CRUD for events |
| Advisors | `/api/advisors` | CRUD for advisors |

Full interactive documentation is available via Swagger at `/swagger` when the app is running.

---

## 🌐 CORS Configuration

The API is configured to accept requests from:

- `http://localhost:5173` (local Vite/React frontend)
- `https://intercoracoid-tamiko-spasmodically.ngrok-free.dev` (ngrok tunnel)

To add more origins, update the CORS policy in `Program.cs`.

