#!/bin/bash

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

DOCKER_DIR="$PROJECT_ROOT/docker"

export ASPNETCORE_ENVIRONMENT=production

docker compose -f "$DOCKER_DIR/production.docker-compose.yml" up -d --build