# Entity Manager API

A .NET 9.0 Web API for managing entity records with basic CRUD operations.

## Features

- Create new entities
- List all entities
- Search entities by name or phone number
- Delete entities by name or phone number
- In-memory database for testing
- Swagger UI documentation
- CORS enabled
- Structured API responses

## Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the application:

```bash
dotnet run
```

The API will be available at:

- HTTP: http://localhost:5079
- HTTPS: https://localhost:7031

## API Endpoints

### Add Entity

```http
POST /api/entity/add
```

### List All Entities

```http
GET /api/entity/list
```

### Search Entities

```http
GET /api/entity/search?name={name}&phoneNumber={phoneNumber}
```

### Delete Entity

```http
DELETE /api/entity/delete?name={name}&phoneNumber={phoneNumber}
```

## Entity Model

```csharp
public class Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
```

## Technologies

- ASP.NET Core 9.0
- Entity Framework Core
- Swagger/OpenAPI
- PostgreSQL (configured but using InMemory for development)

## Development

The project uses:

- Entity Framework Core InMemory provider for development
- Swagger UI for API documentation and testing
- Standard HTTP status codes and consistent response format
- Logging for error tracking

## Configuration

Connection strings and other settings can be found in `appsettings.json`:

## License

MIT
```