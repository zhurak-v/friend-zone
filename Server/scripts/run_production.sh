#!/bin/bash

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PUBLISH_DIR="$PROJECT_ROOT/publisher"

ENTRY_DLL=$(find "$PUBLISH_DIR" -maxdepth 1 -name "*.dll" | head -n 1)

if [ -z "$ENTRY_DLL" ]; then
  echo "Entry DLL not found in $PUBLISH_DIR"
  exit 1
fi

export ASPNETCORE_ENVIRONMENT=production

dotnet "$ENTRY_DLL"