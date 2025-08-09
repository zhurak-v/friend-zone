#!/bin/bash

set -e

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
APP_PROJECT="$PROJECT_ROOT/src/Application/Application.csproj"
OUTPUT_DIR="$PROJECT_ROOT/publisher"

rm -rf "$OUTPUT_DIR"

dotnet publish "$APP_PROJECT" -c Release -r win-x64 --self-contained true -o "$OUTPUT_DIR"

