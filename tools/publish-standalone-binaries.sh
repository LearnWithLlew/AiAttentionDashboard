#!/usr/bin/env bash
# Build a standalone binary for each platform and attach them to a GitHub release.
#
# Usage: publish-standalone-binaries.sh TAG
# TAG is the release's tag, such as v1.2.3. The gh CLI must be authenticated.
set -euo pipefail

if [ $# -ne 1 ]; then
    echo "usage: $(basename "$0") tag" >&2
    exit 2
fi

tag=$1
version="${tag#v}"

cd "$(dirname "$0")/.."

mkdir archives
for rid in win-x64 win-arm64 linux-x64 linux-arm64 osx-x64 osx-arm64; do
    dotnet publish src/AIAttentionDashboard --configuration Release --runtime "$rid" --self-contained \
        -p:PublishSingleFile=true -p:PackAsTool=false -p:EnableCompressionInSingleFile=true \
        -p:Version="$version" --output "publish/$rid"
    case "$rid" in
        win-*) zip -j "archives/aiattentiondashboard-$rid.zip" "publish/$rid/aiattentiondashboard.exe" ;;
        *)
            chmod +x "publish/$rid/aiattentiondashboard"
            tar -czf "archives/aiattentiondashboard-$rid.tar.gz" -C "publish/$rid" aiattentiondashboard
            ;;
    esac
done
gh release upload "$tag" archives/*
