# Roommater_API

## MySQL Setup

This API uses **MySQL** with EF Core (Pomelo provider).

### 1) Install MySQL
- Install MySQL Server 8.x.
- Ensure MySQL is running on `localhost:3306` (or update connection strings accordingly).

### 2) Create the database
Run in MySQL shell:

```sql
CREATE DATABASE IF NOT EXISTS RoommaterDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE DATABASE IF NOT EXISTS RoommaterDb_Dev CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3) Configure connection strings
`Roommater_API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=RoommaterDb;User=root;Password=;"
}
```

`Roommater_API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=RoommaterDb_Dev;User=root;Password=;"
}
```

Set a secure production password and avoid committing secrets.

### 4) Run migrations
In development, pending migrations are applied automatically on startup.

You can also apply manually:

```bash
dotnet ef database update --project /home/runner/work/Roommater_API/Roommater_API/Roommater_API/Roommater_API.csproj
```

## JWT Configuration

Set `Jwt:Key` from a secure source (for example environment variable `Jwt__Key` or a secret manager) before running in non-development environments.
