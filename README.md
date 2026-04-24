# BioTablero CM

Backend for BioTablero's Community Monitoring module.

Built with .NET 8.0.

## Requirements

* [.NET 8.0 (SDK)](https://dotnet.microsoft.com/en-us/download)

## Getting Started

### Environment Variables

Generate a `.env` file with the project parameters. You can generate the file based on the `sample.env` example.

### Run containers

```sh
docker compose -f docker-compose-dev.yml -p cm-local up
```

> **Note:** When running this command for the first time, it's recommended to verify that the project bucket was built correctly with the `docker logs bt-cm-localstack` command. If you encounter an execution permission error, run these commands:

```sh
# Add execution permissions to custom script
docker exec bt-cm-localstack chmod +x /etc/localstack/init/ready.d/script.sh
# Restart localstack container
docker restart bt-cm-localstack
```

### Install dependencies

```sh
# Install EF Core tools
dotnet tool install --global dotnet-ef --version 8.0.18
# Install project dependencies
dotnet restore
```

> **Note:** After installing `dotnet-ef`, verify that the command works and is correctly configured in the `PATH` variable.

### Run database migrations

```sh
dotnet ef database update --startup-project src/WebApi --project src/Infrastructure --context GeneralContext
```

### Run development server

```sh
dotnet run --project src/WebApi/WebApi.csproj
```

Check Swagger docs [here](http://localhost:8001/swagger/index.html).

## Code checks

### dotnet-format

Formats code to match `.editorconfig` settings.

```sh
# Check format
dotnet format --verify-no-changes
# Fix format issues
dotnet format
```

### Warnings

Check project warnings as errors

```sh
dotnet build --no-incremental -warnaserror
```

## Database migrations

> Check the format of the generated code before uploading it to the repository

> Replace the `MIGRATION_NAME` variable with a short description in **Pascal Case**

```sh
# List all migrations
dotnet ef migrations list --startup-project src/WebApi --project src/Infrastructure --context GeneralContext
# Generate migration
dotnet ef migrations add $MIGRATION_NAME --startup-project src/WebApi --project src/Infrastructure --output-dir Persistence/Migrations --context GeneralContext
# Apply format rules in Infrastructure project (twice)
dotnet format src/Infrastructure && dotnet format src/Infrastructure
```

If you need to remove the last generated migration, you can do so with the command: `dotnet ef migrations remove --startup-project src/WebApi --project src/Infrastructure --context GeneralContext`.

## Docker

### Build image

```sh
# Build image
docker build -t biotablero-cm-backend:latest .
# Run temporal container
docker run -it --rm --env-file .env --name bt-cm-backend -p 8001:8080 --network=cm-local_bt-search-network biotablero-cm-backend:latest
```