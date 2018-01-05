@echo off

set BATCH_DIR=%~dp0

if not [%1]==[] set SLASH_FRAMEWORK=%~1

if ["%SLASH_FRAMEWORK%"]==[""] (
  echo SLASH_FRAMEWORK variable not set.
  exit
)

set SYMLINK_PATH=%~2
set LIBRARY_NAME=%~3

if [%4] == [] (
  set TARGET_FOLDER=%~3 
) else (
  set TARGET_FOLDER=%~4 
)

for %%f in (%TARGET_FOLDER%) do set TARGET_NAME=%%~nxf

echo Library: %LIBRARY_NAME%
echo Target Folder: %TARGET_FOLDER%
REM echo Target folder %TARGET_FOLDER%, Target Name %TARGET_NAME%

REM Go into target folder and one up to create link
cd /d %SYMLINK_PATH%
if not exist "%TARGET_FOLDER%" mkdir "%TARGET_FOLDER%"
cd /d "%TARGET_FOLDER%"
cd /d ..

REM Get relative path to framework from current directory
set CURRENT_DIR=%CD%
REM echo Current %CURRENT_DIR%

call "%BATCH_DIR%MakeRelative" SLASH_FRAMEWORK "%CURRENT_DIR%"

REM echo Relative Framework %SLASH_FRAMEWORK%

set SOURCE_FOLDER=%SLASH_FRAMEWORK%\Source\%LIBRARY_NAME%\Source
set TARGET_FOLDER=%CD%\%TARGET_NAME%

REM echo Link %TARGET_FOLDER% to %SOURCE_FOLDER%

if exist "%TARGET_FOLDER%" (
    rmdir /s /q "%TARGET_FOLDER%"
    if exist "%TARGET_FOLDER%" (
        del /f /q "%TARGET_FOLDER%"
    )
)

mklink /D "%TARGET_FOLDER%" "%SOURCE_FOLDER%"

if %ERRORLEVEL% neq 0 (
    echo Symlink couldn't be created, try running the batch file as Administrator
    exit
)

echo -------------------------

cd /d "%BATCH_DIR%"