# BioTablero CM

Backend for BioTablero's Community Monitoring module.

Built with .NET 8.0.

## Requirements

* [.NET 8.0 (SDK)](https://dotnet.microsoft.com/en-us/download)

## Configuration (Development)

### Environment Variables

Generate a `.env` file with the project parameters. You can generate the file based on the `sample.env` example.

### Run containers

```sh
docker compose -f docker-compose-dev.yml up
```

### Code format

#### dotnet-format

Formats code to match `.editorconfig` settings. Install it with `dotnet tool install -g dotnet-format` command.

```sh
# Check format
dotnet format --verify-no-changes
# Fix format issues
dotnet format
```

#### Warnings

Check project warnings as errors

```sh
dotnet build --no-incremental -warnaserror
```

### Run

```sh
# Download dependencies
dotnet restore
# Build project
dotnet build
# Run project
dotnet run --no-build --project src/WebApi/WebApi.csproj
```

Check Swagger docs [here](http://localhost:8001/swagger/index.html).

## Docker

### Build image

```sh
docker build -t biotablero-cm:latest .
```