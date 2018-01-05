SETLOCAL

set BATCH_DIR=%~dp0

set SYMLINK_PATH=%~1
set CONFIG_FILE=%~2

REM Get relative path to slash framework
set SLASH_FRAMEWORK=%BATCH_DIR%..\..

REM Reflink sources
echo Create symlinks to Slash Framework libraries in '%SYMLINK_PATH%'
echo ################################################################
if not exist "%SYMLINK_PATH%" (
    mkdir "%SYMLINK_PATH%"
)

for /F "tokens=*" %%A in ('type "%CONFIG_FILE%"') do (
  call "%BATCH_DIR%SymlinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" "%SYMLINK_PATH%" %%A
)

ENDLOCAL