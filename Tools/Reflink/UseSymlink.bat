REM @echo off

SETLOCAL

set BATCH_DIR=%~dp0

if not [%1]==[] set SLASH_FRAMEWORK=%~1

if ["%SLASH_FRAMEWORK%"]==[""] (
  echo SLASH_FRAMEWORK variable not set.
  exit
)

set SYMLINK_PATH=%~2

set CONFIG_FILE=%~3

REM Reflink sources
mkdir "%SYMLINK_PATH%"
cd "%SYMLINK_PATH%"

for /F "tokens=*" %%A in ('type "%CONFIG_FILE%"') do (
  call "%BATCH_DIR%SymlinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" %%A
)

cd "%BATCH_DIR%"

ENDLOCAL