set BATCH_DIR=%~dp0

REM Adjust this path to point to Unity Project folder
set SYMLINK_PATH=%BATCH_DIR%../Assets/SlashFramework/

REM Adjust this path to point to Slash Framework folder
set SLASH_FRAMEWORK_PATH=%BATCH_DIR%../Slash.Framework/

REM Symlink framework
call "%SLASH_FRAMEWORK_PATH%Tools/Symlink/UseSymlink.bat" "%SYMLINK_PATH%" "%BATCH_DIR%SlashLibraries.txt"