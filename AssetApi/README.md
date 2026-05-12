# AssetApi

A .NET 10 Web API for managing assets.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Getting Started

```bash
dotnet run --project AssetApi
```

The API will start on `https://localhost:7xxx` / `http://localhost:5xxx` (see console output).

Interactive API docs are available at: `https://localhost:<port>/scalar/v1`

## Authentication

The API uses JWT Bearer authentication. Obtain a token via the login endpoint:

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin123!"
}
```

Seed credentials:

| Username | Password    | Role   |
|----------|-------------|--------|
| admin    | Admin123!   | Admin  |
| viewer   | Viewer123!  | Viewer |

Use the returned `token` as a Bearer token in subsequent requests:

```
Authorization: Bearer <token>
```

## Endpoints

### Auth

| Method | Path             | Auth | Description          |
|--------|------------------|------|----------------------|
| POST   | /api/auth/login  | No   | Obtain a JWT token   |

### Assets

| Method | Path               | Auth         | Description              |
|--------|--------------------|--------------|--------------------------|
| GET    | /api/assets        | Required     | List all assets          |
| GET    | /api/assets/{id}   | Required     | Get a single asset       |
| POST   | /api/assets        | Admin only   | Create a new asset       |
| PUT    | /api/assets/{id}   | Admin only   | Update an asset          |
| DELETE | /api/assets/{id}   | Admin only   | Delete an asset          |

#### Query Parameters for GET /api/assets

| Parameter | Type   | Description                            |
|-----------|--------|----------------------------------------|
| status    | string | Filter by status: Active, InMaintenance, Retired |
| type      | string | Filter by asset type (e.g. "Laptop")   |

## Configuration

JWT settings are in `appsettings.json`. The `Key` **must** be replaced with a strong secret before deploying to any non-development environment.

## Database

SQLite is used for persistence. The database file (`assets.db`) is created and seeded automatically on first run.

To reset to a clean state, delete `assets.db` and restart the application.
