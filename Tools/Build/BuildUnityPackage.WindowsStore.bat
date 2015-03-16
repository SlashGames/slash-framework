REM Clean and build solution
REM NET40_HOME is the path to your local .NET Framework installation (e.g. C:\Windows\Microsoft.NET\Framework\v4.0.30319\)
%NET40_HOME%MSBuild.exe ../../Source/Slash.Framework.WindowsStore.sln /property:Configuration="Debug" /p:Platform="x86" /t:Clean;Build

REM Remove superfluous referenced libraries files.
RM ..\..\Bin\Slash.Unity.Common.WindowsStore\x86\Debug\WinRTLegacy*

REM Copy dlls from Unity.Common
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Common.WindowsStore/x86/Debug" "Slash.Framework/Assets/Plugins/WSA"

REM Build package
"%UNITY_PATH%Unity.exe" -batchmode -projectPath %~dp0Slash.Framework -exportPackage Assets %~dp0Slash.Framework.WindowsStore.unitypackage -quit