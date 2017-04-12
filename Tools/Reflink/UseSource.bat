set BATCH_DIR=%~dp0

set UNITY_PATH=%~1
set CONFIG=%~2

REM Remove reflink to framework
rmdir /s /q "%UNITY_PATH%/Assets/Reflink/Slash Framework"

REM Restore framework
git checkout -- "%UNITY_PATH%\Assets\Slash Framework\*.*"

cd %BATCH_DIR%