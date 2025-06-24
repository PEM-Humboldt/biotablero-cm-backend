ARG VERSION=8.0.411-alpine3.22-amd64
ARG ASP_VERSION=8.0.17-alpine3.22-amd64

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env

WORKDIR /app

## Copy all source code
COPY . .

## Restore dependencies
RUN dotnet restore

## Build project and check warnings
RUN dotnet build --no-incremental -warnaserror

## Publish project
RUN dotnet publish ./src/WebApi/WebApi.csproj \
  -c Release \
  --no-restore \
  -o ./output

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:$ASP_VERSION

# Install packages
RUN apk add --no-cache curl

## Copy compiled project
WORKDIR /app
COPY --from=build-env /app/output .

## User setup
RUN chown -R app /app
USER app

## Configure environment variables
ENV DOTNET_RUNNING_IN_CONTAINER=true \
  DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 \
  ASPNETCORE_HTTP_PORTS=8080

## Open port
EXPOSE 8080

## Docker health check setup
HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --silent --fail http://localhost:8080/health || exit 1

## Execute program
ENTRYPOINT ["dotnet", "WebApi.dll"]
