ARG VERSION=10.0.400-alpine3.24-amd64
ARG ASP_VERSION=10.0.11-alpine3.24-amd64

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env

## Environment variables
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  DOTNET_NOLOGO=1

WORKDIR /app

## Copy solution and project files for dependency caching
COPY *.slnx .
COPY Directory.Build.props .
COPY src/Application/*.csproj ./src/Application/
COPY src/Core/*.csproj ./src/Core/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/WebApi/*.csproj ./src/WebApi/

## Restore dependencies
RUN dotnet restore

## Copy all source code
COPY . .

## Publish project
RUN dotnet publish ./src/WebApi/WebApi.csproj \
  -c Release \
  -warnaserror \
  -o ./output

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:$ASP_VERSION

# Install packages
RUN apk add --no-cache krb5-libs curl fontconfig

## Copy compiled project and assign ownership
WORKDIR /app
COPY --from=build-env --chown=app:app /app/output .

## User setup
USER app

## Environment variables
ENV ASPNETCORE_HTTP_PORTS=8080

## Open port
EXPOSE 8080

## Docker health check setup
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl --silent --fail http://localhost:8080/health/live || exit 1

## Execute program
ENTRYPOINT ["dotnet", "WebApi.dll"]
