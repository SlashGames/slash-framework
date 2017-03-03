REM Setup
REM Install perl on your system, if not already existent: https://www.perl.org/get.html
REM Set environment variables:
REM NET40_HOME is the path to your local .NET Framework installation (e.g. C:\Windows\Microsoft.NET\Framework\v4.0.30319)
REM UNITY_PATH is the path to your Unity3D installation (e.g. C:\Program Files\Unity)

REM Clean and build solution
"%NET40_HOME%\MSBuild.exe" ../../Source/Slash.Framework.sln /property:Configuration="Debug NoLog4Net" /p:Platform="Any CPU" /t:Clean;Build

REM Remove superfluous Unity.Editor.Common files.
MD ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep
MOVE ..\..\Bin\Slash.Unity.Editor.Common\Debug\Slash.Unity.Editor.Common.* ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep
DEL /Q ..\..\Bin\Slash.Unity.Editor.Common\Debug\*
MOVE ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep\Slash.Unity.Editor.Common.* ..\..\Bin\Slash.Unity.Editor.Common\Debug
RD ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep

REM Copy dlls from Unity.Common
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Common/AnyCPU/Debug NoLog4Net" "Slash.Framework/Assets/Plugins"
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Editor.Common/Debug" "Slash.Framework/Assets/Editor/Plugins"

REM Copy non-DLL files
xcopy /h /s /y "../../Source/Slash.Unity.Export/Source/Assets" "Slash.Framework/Assets"

REM Build package
"%UNITY_PATH%\Editor\Unity.exe" -batchmode -projectPath "%~dp0Slash.Framework" -exportPackage Assets "%~dp0Slash.Framework.unitypackage" -quit