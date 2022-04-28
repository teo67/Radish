@echo off
:: build your own radish
setlocal enabledelayedexpansion
mkdir Radish
dotnet publish -c Release -r %1 --self-contained false -o Radishs
endlocal