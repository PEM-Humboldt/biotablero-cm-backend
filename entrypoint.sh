#!/bin/sh
set -e

if [ "$RUN_MIGRATIONS" = "true" ]; then
  echo "Running database migrations..."
  /app/efbundle --connection "$CS_MAIN"
fi

echo "Starting application..."
exec dotnet WebApi.dll
