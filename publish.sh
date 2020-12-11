#!/usr/bin/sh

# usage: ./publish.sh <api key>

set -e

cd FancyPen
dotnet build -c Release
dotnet pack -c Release

cd ../FancyPen.json
dotnet build -c Release
dotnet pack -c Release

cd ..
dotnet test

dotnet nuget push ./bin/Release/FancyPen.0.0.2.nupkg --api-key $1 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./bin/Release/FancyPen.Json.0.0.2.nupkg --api-key $1 --source https://api.nuget.org/v3/index.json
