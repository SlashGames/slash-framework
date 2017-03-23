set BATCH_DIR=%~dp0

REM Adjust this path to point to Unity Project folder
set UNITY_PROJECT_PATH=%BATCH_DIR%/../..

REM Reflink framework
call "%SLASH_FRAMEWORK%/Tools/Reflink/SetupFramework.bat" "%SLASH_FRAMEWORK%" "%UNITY_PROJECT_PATH%" "%BATCH_DIR%/SlashLibraries.txt"

cd %BATCH_DIR%

REM Delete dlls
del "%UNITY_PROJECT_PATH%/Assets/Plugins/Slash.*.dll"
del "%UNITY_PROJECT_PATH%/Assets/Editor/Plugins/Slash.*.dll"

cd %BATCH_DIR%