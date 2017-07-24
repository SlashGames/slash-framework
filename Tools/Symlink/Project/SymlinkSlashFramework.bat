set BATCH_DIR=%~dp0

REM Adjust this path to point to Unity Project folder
set SYMLINK_PATH=%BATCH_DIR%..\Assets\SlashFramework\

REM Symlink framework
call "../Slash.Framework/Tools/Symlink/UseSymlink.bat" "%SYMLINK_PATH%" "%BATCH_DIR%SlashLibraries.txt"