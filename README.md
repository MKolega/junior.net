# AbySalto.Junior — Run & Test Guide

This document explains how to run the ASP.NET project, set up the local SQL Server database, apply EF Core migrations, and execute the included HTTP tests.

Prerequisites
- Docker (for local SQL Server container)
- .NET 9 SDK (matching project TargetFramework)

1) Start local SQL Server (development)

Run a disposable SQL Server container (development use only):

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=Your_strong_password_123!" \
  -p 1433:1433 \
  --name Abysalto \
  -d mcr.microsoft.com/mssql/server:2022-latest

# or if already created:
docker start Abysalto
```

2) Verify the connection string

Open AbySalto.Junior/appsettings.Development.json and ensure the `DefaultConnection` matches the container settings. Example used in this project:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=AbySalto;User Id=sa;Password=Your_strong_password_123!;TrustServerCertificate=True;Encrypt=False;"
}
```

3) Install EF Core CLI (if you need to create migrations)

Install a version of `dotnet-ef` compatible with the EF packages in the project (the project uses EF Core 9.x):

```bash
# if previously installed and mismatched, uninstall first
dotnet tool uninstall --global dotnet-ef || true
dotnet tool install --global dotnet-ef --version 9.0.2
dotnet ef --version
```

4) Apply migrations (create DB + schema)

This repository already contains an initial migration under `AbySalto.Junior/Migrations`. To create the database and apply migrations run from the workspace root:

```bash
cd /workspaces/junior.net
dotnet ef database update --project AbySalto.Junior --startup-project AbySalto.Junior
```

If you change models later, create a new migration:

```bash
dotnet ef migrations add AddSomething --project AbySalto.Junior --startup-project AbySalto.Junior
dotnet ef database update --project AbySalto.Junior --startup-project AbySalto.Junior
```

5) Run the API

From the project folder:

```bash
cd /workspaces/junior.net/AbySalto.Junior
dotnet run --urls http://localhost:5074
```

6) Test the HTTP endpoints

- Use the included `AbySalto.Junior/AbySalto.Junior.http` file (VS Code REST Client) to run the example `POST`, `GET`, and `PATCH` requests.
- Or use curl examples:

Create an order (POST):

```bash
curl -sS -X POST http://localhost:5074/api/Restaurant \
  -H 'Content-Type: application/json' \
  -d '{
    "customerName":"John Doe",
    "paymentMethod":"Cash",
    "address":"Main Street 1",
    "phoneNumber":"+3850912223333",
    "notes":"No onions",
    "currency":"USD",
    "items":[{"name":"Burger","price":10.0,"quantity":2}]
  }' | jq '.'
```

List orders (sorted by amount descending):

```bash
curl -sS -G 'http://localhost:5074/api/Restaurant' --data-urlencode 'sort=amount_desc' -H 'Accept: application/json' | jq '.'
```

Update order status (PATCH):

```bash
curl -sS -X PATCH http://localhost:5074/api/Restaurant/1/status \
  -H 'Content-Type: application/json' \
  -d '{"status":2}' -i
```

