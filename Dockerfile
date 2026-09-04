ARG VERSION=10.0.400-alpine3.24-amd64
ARG ASP_VERSION=10.0.11-alpine3.24-amd64

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env

WORKDIR /app

## Copy solution and project files
ADD *.slnx .
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

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:$ASP_VERSION

# Install packages
RUN apk add --no-cache krb5-libs curl fontconfig

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
HEALTHCHECK --interval=30s --timeout=30s --retries=3 CMD curl --silent --fail http://localhost:8080/health/live || exit 1

## Execute program
ENTRYPOINT ["dotnet", "WebApi.dll"]
