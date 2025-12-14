#!/usr/bin/env bash
set -euo pipefail

# Cross-publish the WinForms app for Windows and zip the output.
# Usage: ./scripts/publish-windows.sh [rid] [self-contained:true|false] [single-file:true|false]

PROJ="GuitarTranspositionTool/GuitarTranspositionTool.csproj"
RID="${1:-win-x64}"
SELF="${2:-true}"
SINGLE="${3:-false}"
OUT="$(pwd)/publish/$RID"

mkdir -p "$OUT"
CMD=(dotnet publish "$PROJ" -c Release -r "$RID" -o "$OUT")
if [ "$SELF" = "true" ]; then
  CMD+=(--self-contained true)
fi
if [ "$SINGLE" = "true" ]; then
  CMD+=(-p:PublishSingleFile=true)
fi

echo "Running: ${CMD[*]}"
"${CMD[@]}"

ZIP_OUT="$(pwd)/publish/GuitarTranspositionTool-$RID.zip"
if command -v zip >/dev/null 2>&1; then
  (cd "$OUT" && zip -r "$ZIP_OUT" .)
  echo "Zipped publish to: $ZIP_OUT"
else
  echo "zip not found; publish output available at: $OUT"
fi

echo "Done. Copy the zip or the contents of $OUT to a Windows machine and run GuitarTranspositionTool.exe"
