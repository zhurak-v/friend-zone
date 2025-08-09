#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

chmod +x "$SCRIPT_DIR/build_production_win.sh"
chmod +x "$SCRIPT_DIR/run_production.sh"

"$SCRIPT_DIR/build_production_win.sh"
"$SCRIPT_DIR/run_production.sh"
