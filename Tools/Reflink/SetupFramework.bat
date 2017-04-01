REM @echo off

SETLOCAL

set BATCH_DIR=%~dp0

if not [%1]==[] set SLASH_FRAMEWORK=%~1

if ["%SLASH_FRAMEWORK%"]==[""] (
  echo SLASH_FRAMEWORK variable not set.
  exit
)

set UNITY_PROJECT_DIR=%~2

set CONFIG_FILE=%~3

cd "%UNITY_PROJECT_DIR%/Assets"
mkdir Slash.Framework

cd Slash.Framework

for /F "tokens=*" %%A in ('type "%CONFIG_FILE%"') do (
  call "%BATCH_DIR%LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" "%%A"
)

cd "%BATCH_DIR%"

ENDLOCAL