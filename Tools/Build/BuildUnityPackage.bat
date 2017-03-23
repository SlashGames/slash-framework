REM @echo off

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

REM Clean and build solution
"%NET40_HOME%\MSBuild.exe" ../../Source/Slash.Framework.sln /property:Configuration="%CONFIG%" /p:Platform="Any CPU" /t:Clean;Build

REM Clear output path
rmdir /s /q Slash.Framework

REM Remove superfluous Unity.Editor.Common files.
MD "..\..\Bin\Slash.Unity.Editor.Common\AnyCPU\%CONFIG%\keep"
MOVE "..\..\Bin\Slash.Unity.Editor.Common\AnyCPU\%CONFIG%\Slash.Unity.Editor.Common.*" "..\..\Bin\Slash.Unity.Editor.Common\AnyCPU\%CONFIG%\keep"
DEL /Q "..\..\Bin\Slash.Unity.Editor.Common\AnyCPU\%CONFIG%\*"
MOVE "..\..\Bin\Slash.Unity.Editor.Common\AnyCPU\%CONFIG%\keep\Slash.Unity.Editor.Common.*" "..\..\Bin\Slash.Unity.Editor.Common\AnyCPU\%CONFIG%"
RD "..\..\Bin\Slash.Unity.Editor.Common\AnyCPU\%CONFIG%\keep"

REM Copy dlls from Unity.Common
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Common/AnyCPU/%CONFIG%" "Slash.Framework/Assets/Plugins"
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Editor.Common/AnyCPU/%CONFIG%" "Slash.Framework/Assets/Editor/Plugins"

REM Copy non-DLL files
xcopy /h /s /i /y "../../Ext/StrangeIoC/Source" "Slash.Framework/Assets/StrangeIoC"
xcopy /h /s /i /y "../../Source/Slash.Unity.StrangeIoC/Source" "Slash.Framework/Assets/Slash Framework/Slash.Unity.StrangeIoC"
xcopy /h /s /y "../../Source/Slash.Unity.Export/Source/Assets" "Slash.Framework/Assets"

REM Build package
"%UNITY_PATH%\Editor\Unity.exe" -batchmode -projectPath "%~dp0Slash.Framework" -exportPackage "Assets/Slash Framework" "Assets/Plugins" "Assets/Editor" "%~dp0%PACKAGE%.unitypackage" -logFile Unity.log -quit