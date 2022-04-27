@echo off
:: this command publishes the current code under a version number on all supported platforms
setlocal enabledelayedexpansion
mkdir release
SET first=
FOR %%N IN (win-x64 win-x86 win-arm win-arm64 linux-x64 linux-arm linux-arm64 osx-x64) DO (
    SET first=!first! ./release/%1/%%N/v%1-%%N.zip
    dotnet publish -c Release -r %%N --self-contained false -o release/%1/%%N/Radish
    cd release
    cd %1
    cd %%N
    :: we need to cd into the folder so 7zip doesnt zip up the surrounding folders as well
    7z a -tzip v%1-%%N.zip Radish
    cd ../../..
    rmdir %cd%\release\%1\%%N\Radish /s /q
)
echo !first!
gh release create v%1 !first! -t v%1 --notes-file ./releasenotes/v%1.md
rmdir %cd%\release /s /q
endlocal