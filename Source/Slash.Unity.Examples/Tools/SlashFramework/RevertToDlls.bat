set BATCH_DIR=%~dp0

set UNITY_PATH=%BATCH_DIR%../..

REM Remove reflink to framework
rmdir /s /q "%UNITY_PATH%/Assets/Slash.Framework"

REM Restore dlls
git checkout -- "%UNITY_PATH%/Assets/Plugins/Slash.*.dll"
git checkout -- "%UNITY_PATH%/Assets/Plugins/Slash.*.dll.meta"
git checkout -- "%UNITY_PATH%/Assets/Editor/Plugins/Slash.*.dll"
git checkout -- "%UNITY_PATH%/Assets/Editor/Plugins/Slash.*.dll.meta"

REM Restore Unity modules
git checkout -- "%UNITY_PATH%/Assets/Slash Framework/Slash.Unity.Common.meta"
git checkout -- "%UNITY_PATH%/Assets/Slash Framework/Slash.Unity.Common/*.*"
git checkout -- "%UNITY_PATH%/Assets/Slash Framework/Editor/Slash.Unity.Editor.Common.meta"
git checkout -- "%UNITY_PATH%/Assets/Slash Framework/Editor/Slash.Unity.Editor.Common/*.*"
git checkout -- "%UNITY_PATH%/Assets/Slash Framework/Slash.Unity.StrangeIoC.meta"
git checkout -- "%UNITY_PATH%/Assets/Slash Framework/Slash.Unity.StrangeIoC/*.*"

cd %BATCH_DIR%