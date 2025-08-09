#!/bin/bash

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
DOCKER_DIR="$PROJECT_ROOT/docker"

GIT_HASH=$(git -C "$PROJECT_ROOT" rev-parse --short HEAD)
TAG="aspnet_server_image:$GIT_HASH"

export ASPNETCORE_ENVIRONMENT=production
set ASPNETCORE_ENVIRONMENT=production

docker build -t "$TAG" -f "$DOCKER_DIR/dockerfiles/Production.Server.Dockerfile" "$PROJECT_ROOT"

# docker run -d \
#   --name aspnet_server_container \
#   -p 7777:7777 \
#   -e ASPNETCORE_ENVIRONMENT=production \
#   "$TAG"