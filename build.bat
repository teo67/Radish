@echo off
:: build your own radish
setlocal enabledelayedexpansion
dotnet build
mkdir Radish
cd Radish
mkdir bin
mkdir lib
cd ..
dotnet publish -c Release -r %1 --self-contained false -o %cd%\Radish\bin
xcopy /s %cd%\Radish-Standard-Library %cd%\Radish\lib
endlocal