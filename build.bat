@echo off
:: build your own radish
setlocal enabledelayedexpansion
mkdir build
FOR %%N IN (win-x64 osx-x64) DO (
    dotnet publish -c Release -r %%N --self-contained false -o build/%%N/Radish
    cd build
    cd %%N
    :: we need to cd into the folder so 7zip doesnt zip up the surrounding folders as well
    7z a -tzip %%N.zip Radish
    cd ../..
    rmdir %cd%\build\%%N\Radish /s /q
)
endlocal