REM @echo off

set BATCH_DIR=%~dp0

set UNITY_PROJECT_PATH=%BATCH_DIR%..\..

if ["%SLASH_FRAMEWORK%"]==[""] (
  echo SLASH_FRAMEWORK variable not set.
  exit
)

REM Reflink framework
call "%SLASH_FRAMEWORK%/Tools/Reflink/SetupFramework.bat" "%SLASH_FRAMEWORK%" "%UNITY_PROJECT_PATH%" "%BATCH_DIR%SlashLibraries.txt"

cd "%BATCH_DIR%"

REM Delete dlls
del "%UNITY_PROJECT_PATH%\Assets\Plugins\Slash*.dll"
del "%UNITY_PROJECT_PATH%\Assets\Editor\Plugins\Slash*.dll"

REM Delete Slash.Unity.StrangeIoC
rmdir /s /q "%UNITY_PROJECT_PATH%\Assets\Slash Framework\Slash.Unity.Common"
rmdir /s /q "%UNITY_PROJECT_PATH%\Assets\Slash Framework\Slash.Unity.StrangeIoC"
rmdir /s /q "%UNITY_PROJECT_PATH%\Assets\Slash Framework\Editor\Slash.Unity.Editor.Common"

cd "%BATCH_DIR%"