ARG VERSION=8.0.420-alpine3.23-amd64
ARG ASP_VERSION=8.0.26-alpine3.23-amd64

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env

WORKDIR /app

## Copy solution and project files
ADD *.sln .
ADD Directory.Build.props .
ADD /src/Application/*.csproj ./src/Application/
ADD /src/Core/*.csproj ./src/Core/
ADD /src/Infrastructure/*.csproj ./src/Infrastructure/
ADD /src/WebApi/*.csproj ./src/WebApi/

## Restore dependencies
RUN dotnet restore

## Copy all source code
COPY . .

## Build project and check warnings
RUN dotnet build \
  -c Release \
  --no-incremental \
  -warnaserror

## Publish project
RUN dotnet publish ./src/WebApi/WebApi.csproj \
  -c Release \
  --no-build \
  --no-restore \
  -o ./output

## Build EF Core migration bundle (self-contained, Alpine/musl compatible)
RUN dotnet tool install --global dotnet-ef --version 8.0.18
ENV PATH="$PATH:/root/.dotnet/tools"
RUN export CS_MAIN="Host=localhost;Port=5432;Username=dev;Password=dev;Database=dev" && \
  export KC_BASE_URL="http://localhost" && \
  export KC_REALM="dev" && \
  export SMTP_PORT="25" && \
  dotnet ef migrations bundle \
  --startup-project ./src/WebApi/WebApi.csproj \
  --project ./src/Infrastructure/Infrastructure.csproj \
  --context GeneralContext \
  --self-contained \
  -r linux-musl-x64 \
  -o ./efbundle

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:$ASP_VERSION

# Install packages
RUN apk add --no-cache curl

## Copy compiled project
WORKDIR /app
COPY --from=build-env /app/output .

## Copy migration bundle
COPY --from=build-env /app/efbundle /app/efbundle
RUN chmod +x /app/efbundle

## Copy entrypoint script
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

## User setup
RUN chown -R app /app
USER app

## Configure environment variables
ENV DOTNET_RUNNING_IN_CONTAINER=true \
  DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 \
  ASPNETCORE_HTTP_PORTS=8080 \
  RUN_MIGRATIONS=false

## Open port
EXPOSE 8080

## Docker health check setup
HEALTHCHECK --interval=30s --timeout=30s --retries=3 CMD curl --silent --fail http://localhost:8080/health/live || exit 1

## Execute program
ENTRYPOINT ["/app/entrypoint.sh"]
