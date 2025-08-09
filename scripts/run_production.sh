#!/bin/bash

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PUBLISH_DIR="$PROJECT_ROOT/publisher"

ENTRY_DLL="$PUBLISH_DIR/Application.dll"

if [ ! -f "$ENTRY_DLL" ]; then
  echo "Entry DLL not found: $ENTRY_DLL"
  exit 1
fi

export ASPNETCORE_ENVIRONMENT=production

dotnet "$ENTRY_DLL"
