@echo off
:: this command publishes the current code under a version number on all supported platforms
dotnet publish -c Release -r win-x64 --self-contained false -o releases/%1/win-64
dotnet publish -c Release -r win-x86 --self-contained false -o releases/%1/win-86
dotnet publish -c Release -r win-arm --self-contained false -o releases/%1/win-arm
dotnet publish -c Release -r win-arm64 --self-contained false -o releases/%1/win-arm64
dotnet publish -c Release -r osx-x64 --self-contained false -o releases/%1/osx-64
dotnet publish -c Release -r linux-x64 --self-contained false -o releases/%1/linux-64
dotnet publish -c Release -r linux-arm --self-contained false -o releases/%1/linux-arm
dotnet publish -c Release -r linux-arm64 --self-contained false -o releases/%1/linux-arm64