@echo off

REM Setup
REM Install perl on your system, if not already existent: https://www.perl.org/get.html
REM Set environment variables:
REM NET40_HOME is the path to your local .NET Framework installation (e.g. C:\Windows\Microsoft.NET\Framework\v4.0.30319)
REM UNITY_PATH is the path to your Unity3D installation (e.g. C:\Program Files\Unity)

set UNITY_PATH=%~1
set CONFIG=%~2
set PACKAGE=%~3

if ["%UNITY_PATH%"] == [""] (
	echo UNITY_PATH not set
	goto :EOF
)

REM Make sure logs directory exists
if not exist "%~dp0logs" mkdir "%~dp0logs"

echo Build libraries...

REM Clean and build solution
"%NET40_HOME%\MSBuild.exe" ../../Source/Slash.Framework.sln /verbosity:minimal /property:Configuration="%CONFIG%" /p:Platform="Any CPU" /t:Clean;Build > "logs/BuildLibraries.log"

set BUILD_STATUS=%ERRORLEVEL%
if %BUILD_STATUS%==0 (
  echo ...success
)
if not %BUILD_STATUS%==0 (
  echo ...failed with error code %BUILD_STATUS%
  goto :EOF
)

REM Clear output path
if exist Slash.Framework rmdir /s /q Slash.Framework

set UNITY_SLASH_FRAMEWORK_PATH=Slash.Framework/Assets/SlashFramework

echo Copy dlls to package...

REM Copy dlls from Unity.Common
perl BuildUnityPackage.pl "../../Bin" "%CONFIG%" "UnityPackageLibraries.txt" "%UNITY_SLASH_FRAMEWORK_PATH%/Plugins" > "logs/CopyDlls.log"

echo Copy source files to package...

REM Copy non-DLL files
xcopy /h /s /q /i /y "../../Source/Slash.Unity.Common/Source" "%UNITY_SLASH_FRAMEWORK_PATH%/Slash.Unity.Common" > nul
xcopy /h /s /q /i /y "../../Source/Slash.Unity.Editor.Common/Source" "%UNITY_SLASH_FRAMEWORK_PATH%/Editor/Slash.Unity.Editor.Common" > nul
xcopy /h /s /q /i /y "../../Ext/StrangeIoC/Source" "Slash.Framework/Assets/StrangeIoC" > nul
xcopy /h /s /q /i /y "../../Source/Slash.Unity.StrangeIoC/Source" "%UNITY_SLASH_FRAMEWORK_PATH%/Slash.Unity.StrangeIoC" > nul
xcopy /h /s /q /i /y "../../Source/Slash.Unity.StrangeIoC.Video/Source" "%UNITY_SLASH_FRAMEWORK_PATH%/Slash.Unity.StrangeIoC.Video" > nul
xcopy /h /s /q /y "../../Source/Slash.Unity.Export/Source/Assets" "Slash.Framework/Assets" > nul

echo Build addon unity packages...

REM Build addons
mkdir "%~dp0%UNITY_SLASH_FRAMEWORK_PATH%/Addons"
"%UNITY_PATH%\Editor\Unity.exe" -batchmode -projectPath "%~dp0Slash.Framework" -exportPackage "Assets/SlashFramework/Slash.Unity.StrangeIoC" "Assets/SlashFramework/Slash.Unity.StrangeIoC.Video" "%~dp0%UNITY_SLASH_FRAMEWORK_PATH%/Addons/Slash.Unity.StrangeIoC.unitypackage" -logFile "logs/AddonStrangeIoC.log" -quit

set BUILD_STATUS=%ERRORLEVEL%
if %BUILD_STATUS%==0 (
  echo ...success
)
if not %BUILD_STATUS%==0 (
  echo ...failed with error code %BUILD_STATUS%
  goto :EOF
)

REM Remove addons
rmdir /s /q "%UNITY_SLASH_FRAMEWORK_PATH%/Slash.Unity.StrangeIoC"
rmdir /s /q "%UNITY_SLASH_FRAMEWORK_PATH%/Slash.Unity.StrangeIoC.Video"

echo Build main unity package...

REM Build package
"%UNITY_PATH%\Editor\Unity.exe" -batchmode -projectPath "%~dp0Slash.Framework" -exportPackage "Assets/SlashFramework" "%~dp0%PACKAGE%.unitypackage" -logFile logs/MainPackage.log -quit

set BUILD_STATUS=%ERRORLEVEL%
if %BUILD_STATUS%==0 (
  echo ...success
)
if not %BUILD_STATUS%==0 (
  echo ...failed with error code %BUILD_STATUS%
  goto :EOF
)