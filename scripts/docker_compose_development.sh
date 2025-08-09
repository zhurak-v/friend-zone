#!/bin/bash

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

DOCKER_DIR="$PROJECT_ROOT/docker"

export ASPNETCORE_ENVIRONMENT=development

docker compose -f "$DOCKER_DIR/development.docker-compose.yml" up -d --build