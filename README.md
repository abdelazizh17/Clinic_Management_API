# Clinic Management API

A layered ASP.NET Core Web API for managing a small clinic: doctors, patients, appointments, prescriptions, medications, specialties, users and roles.

The project is built as a **Junior Backend portfolio project**: it focuses on clean layering, JWT authentication, role-based authorization, validation, consistent error responses, pagination and Docker.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Run Locally](#run-locally)
- [Configuration and Secrets](#configuration-and-secrets)
- [Run with Docker](#run-with-docker)
- [Roles and Authorization](#roles-and-authorization)
- [API Endpoints](#api-endpoints)
- [Pagination](#pagination)
- [Error Handling](#error-handling)
- [Background Jobs](#background-jobs)
- [Health Checks](#health-checks)
- [Database Migrations](#database-migrations)
- [Security Notes](#security-notes)

---

## Features

- JWT authentication with configurable issuer, audience and lifetime.
- Role-based authorization (`Admin`, `Doctor`, `Receptionist`).
- Global fallback policy: every endpoint requires authentication unless it explicitly has `[AllowAnonymous]`.
- CRUD for doctors, patients, appointments, prescriptions, medications and specialties.
- Repository + Unit of Work pattern.
- Generic pagination with `TotalCount`, `TotalPages`, `HasPreviousPage` and `HasNextPage`.
- FluentValidation with automatic request validation.
- Mapster object mapping.
- Global exception handling with consistent `ProblemDetails` responses.
- Custom business exceptions: `NotFoundException` (404), `ConflictException` (409), `BadRequestException` (400).
- Hangfire background job that marks missed appointments as `NoShow`.
- Hangfire dashboard protected by role (or local-only in Development).
- Swagger UI with JWT bearer support.
- Health check endpoint for Docker and hosting platforms.
- Docker + Docker Compose with SQL Server and a persistent volume.

---

## Tech Stack

| Area | Technology |
|---|---|
| Runtime | .NET 10 |
| Web | ASP.NET Core Web API |
| Database | SQL Server |
| ORM | Entity Framework Core 10 |
| Authentication | JWT Bearer |
| Identity | ASP.NET Core Identity |
| Validation | FluentValidation + SharpGrip AutoValidation |
| Mapping | Mapster |
| Background jobs | Hangfire |
| API docs | Swashbuckle / Swagger |
| Containers | Docker + Docker Compose |

---

## Architecture

The project uses a **Layered Architecture** (not Clean Architecture):

```text
HTTP Request
    │
    ▼
Controllers              (routing, authorization, HTTP status codes)
    │
    ▼
Services                 (business rules, validation of business state)
    │
    ▼
Repositories + UnitOfWork (data access, EF Core queries)
    │
    ▼
ClinicDBContext           (SQL Server)
```

Folders:

```text
Clinic_Management_API/
├── Controllers/          HTTP endpoints
├── Core/
│   ├── DTOs/             Request/response models
│   ├── Entities/         Database entities
│   ├── Enums/            Domain enums
│   ├── Exceptions/       Business exceptions
│   ├── Interfaces/       Service and repository contracts
│   ├── Jobs/             Hangfire jobs
│   ├── Mappings/         Mapster configuration
│   ├── Options/          Strongly-typed configuration (JWT, Seed)
│   ├── Services/         Business logic
│   └── Validation/       FluentValidation validators
├── Errors/               Global exception handler and status code pages
├── Infrastructure/
│   ├── Data/             DbContext and entity configurations
│   ├── HealthChecks/     Database health check
│   ├── Repositories/     EF Core repositories and Unit of Work
│   └── Security/         Hangfire dashboard authorization
├── Migrations/           EF Core migrations
├── Program.cs            Application bootstrap / composition root
└── appsettings.json      Non-secret configuration
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express or Docker)
- Optional: Docker Desktop

---

## Run Locally

### 1. Restore and build

```bash
dotnet restore
dotnet build
```

### 2. Configure the connection string

Open `Clinic_Management_API/appsettings.Development.json` and update the SQL Server connection string if needed:

```json
{
  "ConnectionStrings": {
    "cs": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Clinic_Management_Db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True",
    "HangfireConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HangfireDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
  }
}
```

### 3. Configure secrets with User Secrets

Secrets are **not** stored in `appsettings.json`. Use the .NET Secret Manager:

```bash
dotnet user-secrets set "JWT:Key" "<a-random-string-at-least-32-characters>" --project Clinic_Management_API/Clinic_Management_API.csproj

dotnet user-secrets set "Seed:AdminEmail" "admin@clinic.com" --project Clinic_Management_API/Clinic_Management_API.csproj
dotnet user-secrets set "Seed:AdminUserName" "AdminMaster" --project Clinic_Management_API/Clinic_Management_API.csproj
dotnet user-secrets set "Seed:AdminPassword" "<a-strong-admin-password>" --project Clinic_Management_API/Clinic_Management_API.csproj
```

List the configured secrets:

```bash
dotnet user-secrets list --project Clinic_Management_API/Clinic_Management_API.csproj
```

### 4. Apply migrations and run

Migrations run automatically at startup (`Database.MigrateAsync()`), but you can also run them manually:

```bash
dotnet ef database update --project Clinic_Management_API/Clinic_Management_API.csproj
```

Then run:

```bash
dotnet run --project Clinic_Management_API/Clinic_Management_API.csproj
```

Open:

- Swagger: `https://localhost:7055/swagger` or `http://localhost:5106/swagger`
- Hangfire: `https://localhost:7055/hangfire`
- Health: `https://localhost:7055/health`

Log in with the seeded admin account from your User Secrets.

---

## Configuration and Secrets

Configuration is loaded in this order (later sources override earlier ones):

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. User Secrets (Development only)
4. Environment Variables
5. Command-line arguments

### `appsettings.json`

Contains **no secrets**. It only defines the structure:

```json
{
  "ConnectionStrings": { "cs": "", "HangfireConnection": "" },
  "JWT": { "Key": "", "Issuer": "ClinicManagementApi", "Audience": "ClinicManagementClient", "DurationInDays": 7 },
  "Seed": { "AdminEmail": "", "AdminUserName": "", "AdminPassword": "" }
}
```

### Environment variables

On Linux/Docker, `:` is replaced by `__`:

| Configuration key | Environment variable |
|---|---|
| `ConnectionStrings:cs` | `ConnectionStrings__cs` |
| `ConnectionStrings:HangfireConnection` | `ConnectionStrings__HangfireConnection` |
| `JWT:Key` | `JWT__Key` |
| `JWT:Issuer` | `JWT__Issuer` |
| `Seed:AdminEmail` | `Seed__AdminEmail` |
| `Seed:AdminPassword` | `Seed__AdminPassword` |

---

## Run with Docker

The repository contains:

- `Clinic_Management_API/Dockerfile` — multi-stage image for the API.
- `docker-compose.yml` — SQL Server + API + named volume.
- `.env.example` — template for local environment variables.

### 1. Create `.env`

Copy `.env.example` to `.env` and change every value:

```bash
cp .env.example .env
```

```env
SQL_SA_PASSWORD=ChangeMe_Str0ng!Pass
JWT_KEY=ChangeMe_At_Least_32_Characters_Long_Secret
SEED_ADMIN_EMAIL=admin@clinic.com
SEED_ADMIN_USERNAME=AdminMaster
SEED_ADMIN_PASSWORD=ChangeMe_Admin@12345
```

> `.env` is ignored by Git. Never commit it.

### 2. Build and start

```bash
docker compose up --build
```

or in the background:

```bash
docker compose up -d --build
```

### 3. Open

- Swagger: <http://localhost:8080/swagger>
- Hangfire: <http://localhost:8080/hangfire>
- Health: <http://localhost:8080/health>

### 4. Useful commands

```bash
docker compose logs -f api          # follow API logs
docker compose ps                   # list containers
docker compose down                 # stop containers (data stays)
docker compose down -v              # stop and delete the database volume
docker volume ls                    # list volumes
```

### 5. Connect to SQL Server inside the container

```bash
docker compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "$SQL_SA_PASSWORD" -C \
  -Q "SELECT name FROM sys.databases"
```

- `sqlserver` = **service name** from `docker-compose.yml`.
- `sa` = SQL Server administrator account.
- `Clinic_Management_Db` = the application database.
- `SQL_SA_PASSWORD` = value from `.env`.

---

## Roles and Authorization

The API seeds three roles:

| Role | Typical usage |
|---|---|
| `Admin` | Full access, user and role management |
| `Doctor` | Read patients, appointments and medications; create prescriptions |
| `Receptionist` | Manage patients and appointments |

| Controller | GET | POST / PUT / DELETE |
|---|---|---|
| `AuthController` | — | `login`: anonymous<br>`register`: Admin |
| `UsersController` | — | Admin |
| `AppointmentsController` | Admin, Doctor, Receptionist | Admin, Receptionist |
| `DoctorsController` | Admin, Doctor, Receptionist | Admin |
| `PatientsController` | Admin, Doctor, Receptionist | Admin, Receptionist |
| `MedicationsController` | Admin, Doctor, Receptionist | Admin |
| `SpecialtiesController` | Admin, Doctor, Receptionist | Admin |
| `PrescriptionsController` | Admin, Doctor | Doctor |
| Hangfire Dashboard | Development: local only<br>Other: Admin | — |

`Program.cs` defines a **fallback authorization policy**. Any new endpoint is protected automatically; use `[AllowAnonymous]` only when you intentionally want a public endpoint.

---

## API Endpoints

### Auth

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/login` | Log in and receive a JWT |
| POST | `/api/auth/register` | Create a user (Admin only) |

### Users (Admin only)

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/users/assign-role` | Assign a role to a user |
| POST | `/api/users/update-role` | Replace one role with another |

### Doctors

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/doctors?pageNumber=1&pageSize=10&searchTerm=` | Paginated doctors |
| GET | `/api/doctors/{id}` | Doctor by id |
| GET | `/api/doctors/specialty/{specialtyId}` | Doctors by specialty |
| GET | `/api/doctors/most-appointments?isAscending=false` | Doctors ordered by appointment count |
| POST | `/api/doctors` | Create doctor |
| PUT | `/api/doctors/{id}` | Update doctor |
| DELETE | `/api/doctors/{id}` | Delete doctor |

### Patients

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/patients?pageNumber=1&pageSize=10&searchTerm=` | Paginated patients |
| GET | `/api/patients/{id}` | Patient by id |
| GET | `/api/patients/multiple-appointments` | Patients with more than one appointment |
| POST | `/api/patients` | Create patient |
| PUT | `/api/patients/{id}` | Update patient |
| DELETE | `/api/patients/{id}` | Delete patient |

### Appointments

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/appointments?pageNumber=1&pageSize=10&doctorId=&patientId=&appointmentDate=` | Paginated appointments |
| GET | `/api/appointments/{id}` | Appointment by id |
| GET | `/api/appointments/doctor/{doctorId}?dateTime=2026-01-01` | Doctor appointments for a date |
| POST | `/api/appointments` | Create appointment |
| PUT | `/api/appointments/{id}` | Update appointment |
| PUT | `/api/appointments/{appointmentId}/status` | Change appointment status |
| DELETE | `/api/appointments/{id}` | Delete appointment |

### Prescriptions

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/prescriptions?pageNumber=1&pageSize=10&searchTerm=` | Paginated prescriptions |
| GET | `/api/prescriptions/{id}` | Prescription by id |
| GET | `/api/prescriptions/appointment/{appointmentId}` | Prescription by appointment |
| POST | `/api/prescriptions` | Create prescription (Doctor) |
| PUT | `/api/prescriptions/{id}` | Update prescription (Doctor) |
| DELETE | `/api/prescriptions/{id}` | Delete prescription (Doctor) |

### Medications

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/medications?pageNumber=1&pageSize=10&searchTerm=` | Paginated medications |
| GET | `/api/medications/{id}` | Medication by id |
| POST | `/api/medications` | Create medication (Admin) |
| PUT | `/api/medications/{id}` | Update medication (Admin) |
| DELETE | `/api/medications/{id}` | Delete medication (Admin) |

### Specialties

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/specialties?pageNumber=1&pageSize=10&searchTerm=` | Paginated specialties |
| GET | `/api/specialties/{id}` | Specialty by id |
| POST | `/api/specialties` | Create specialty (Admin) |
| PUT | `/api/specialties/{id}` | Update specialty (Admin) |
| DELETE | `/api/specialties/{id}` | Delete specialty (Admin) |

---

## Pagination

Every `GET` collection endpoint returns a consistent shape:

```json
{
  "items": [
    { "id": 1, "fullName": "Dr. Ahmed" }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 37,
  "totalPages": 4,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

- `TotalCount` = total matching records before paging.
- `TotalPages` = `ceil(TotalCount / PageSize)`.
- `PageNumber` must be `>= 1`.
- `PageSize` must be between `1` and `100`.

---

## Error Handling

The API returns `ProblemDetails` for errors.

| Exception | HTTP Status |
|---|---|
| `NotFoundException` | 404 |
| `ConflictException` | 409 |
| `BadRequestException` | 400 |
| Any unhandled exception | 500 |

Example:

```json
{
  "status": 409,
  "title": "Conflict",
  "detail": "Phone number already exists.",
  "instance": "/api/doctors"
}
```

FluentValidation errors are returned automatically as `400 Bad Request`.

---

## Background Jobs

`NoShowAppointmentJob` runs daily through Hangfire. It marks every `Scheduled` appointment whose end time has passed as `NoShow`.

The job uses the server's local time. In Docker, `TZ=Africa/Cairo` is set so the job uses Cairo time.

---

## Health Checks

```http
GET /health
```

Returns `Healthy` when the API can reach SQL Server. Useful for container orchestration.

---

## Database Migrations

```bash
# Add a migration
dotnet ef migrations add <MigrationName> \
  --project Clinic_Management_API/Clinic_Management_API.csproj \
  --startup-project Clinic_Management_API/Clinic_Management_API.csproj

# Apply migrations
dotnet ef database update \
  --project Clinic_Management_API/Clinic_Management_API.csproj \
  --startup-project Clinic_Management_API/Clinic_Management_API.csproj
```

When running EF commands locally, make sure `ASPNETCORE_ENVIRONMENT=Development` so `appsettings.Development.json` is loaded.

---

## Security Notes

- No secrets are committed to Git.
- JWT keys and seeded admin credentials live in User Secrets locally and environment variables in Docker.
- If a key was ever committed, treat it as compromised and rotate it.
- `.env` and `appsettings.*.local.json` are ignored by Git and Docker.
- The Hangfire dashboard is local-only in Development and Admin-only outside Development.
- `Register` is restricted to `Admin`; new users receive no role until an Admin assigns one.
- Use a strong SQL Server `sa` password. Never use the example password in production.

---

## Author

**Abdelaziz** — [GitHub](https://github.com/abdelazizh17)
