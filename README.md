# BioTablero CM

Backend for BioTablero's Community Monitoring module.

Built with .NET 8.0.

## Requirements

* [.NET 8.0 (SDK)](https://dotnet.microsoft.com/en-us/download)

## Configuration (Development)

### Run

```sh
# Download dependencies
dotnet restore
# Build project
dotnet build
# Run project
dotnet run --no-build --project src/WebApi/WebApi.csproj
```

Check Swagger docs [here](http://localhost:5193/swagger/index.html).

## Docker

### Build image

```sh
docker build -t biotablero-cm:latest .
```