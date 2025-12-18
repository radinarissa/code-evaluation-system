#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_PLUGIN_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
MOODLE_PLUGIN_DIR="$HOME/moodle-dev/moodle/mod/assign/submission/codeeval"

# =================================

echo "=============================================="
echo " Syncing Moodle plugin into local Moodle..."
echo " From: $REPO_PLUGIN_DIR"
echo " To:   $MOODLE_PLUGIN_DIR"
echo "=============================================="

mkdir -p "$MOODLE_PLUGIN_DIR"

rsync -av --delete "$REPO_PLUGIN_DIR/" "$MOODLE_PLUGIN_DIR/"

echo "Done! Now visit Moodle or run CLI upgrade if needed:"
echo "  cd ~/moodle-dev/moodle-docker && bin/moodle-docker-compose exec webserver php admin/cli/upgrade.php --non-interactive"
