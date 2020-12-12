#!/usr/bin/sh

# usage: ./publish.sh <api key>

VERSION=0.0.4

set -e

cd FancyPen
dotnet build -c Release
dotnet pack -c Release

cd ../FancyPen.json
dotnet build -c Release
dotnet pack -c Release

cd ..
dotnet test

dotnet nuget push FancyPen/bin/Release/FancyPen.$VERSION.nupkg --api-key $1 --source https://api.nuget.org/v3/index.json --skip-duplicate
dotnet nuget push FancyPen.Json/bin/Release/FancyPen.Json.$VERSION.nupkg --api-key $1 --source https://api.nuget.org/v3/index.json --skip-duplicate
