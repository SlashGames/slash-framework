REM @echo off

SETLOCAL

set BATCH_DIR=%~dp0

set SYMLINK_PATH=%~1
set CONFIG_FILE=%~2

REM Get relative path to slash framework
set SLASH_FRAMEWORK=%BATCH_DIR%..\..
call "%BATCH_DIR%MakeRelative" SLASH_FRAMEWORK "%SYMLINK_PATH%"

REM Reflink sources
mkdir "%SYMLINK_PATH%"
cd "%SYMLINK_PATH%"

for /F "tokens=*" %%A in ('type "%CONFIG_FILE%"') do (
  call "%BATCH_DIR%SymlinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" %%A
)

cd "%BATCH_DIR%"

ENDLOCAL