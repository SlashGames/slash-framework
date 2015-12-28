REM Clean and build solution
REM NET40_HOME is the path to your local .NET Framework installation (e.g. C:\Windows\Microsoft.NET\Framework\v4.0.30319\)
%NET40_HOME%MSBuild.exe ../../Source/Slash.Framework.sln /property:Configuration="Debug NoLog4Net" /p:Platform="Any CPU" /t:Clean;Build

REM Remove superfluous Unity.Editor.Common files.
MD ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep
MOVE ..\..\Bin\Slash.Unity.Editor.Common\Debug\Slash.Unity.Editor.Common.* ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep
DEL /Q ..\..\Bin\Slash.Unity.Editor.Common\Debug\*
MOVE ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep\Slash.Unity.Editor.Common.* ..\..\Bin\Slash.Unity.Editor.Common\Debug
RD ..\..\Bin\Slash.Unity.Editor.Common\Debug\keep

REM Copy dlls from Unity.Common
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Common/AnyCPU/Debug NoLog4Net" "Slash.Framework/Assets/Plugins"
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Editor.Common/Debug" "Slash.Framework/Assets/Editor/Plugins"

REM Build package
"%UNITY_PATH%Unity.exe" -batchmode -projectPath %~dp0Slash.Framework -exportPackage Assets %~dp0Slash.Framework.unitypackage -quit