REM Clean and build solution
%NET40_HOME%\MSBuild.exe ../../Source/Slash.Framework.sln /property:Configuration="Debug NoLog4Net" /p:Platform="Any CPU" /t:Clean;Build

REM Copy dlls from Unity.Common
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Common/AnyCPU/Debug NoLog4Net"

REM Build package
"%UNITY_PATH%Unity.exe" -batchmode -projectPath %~dp0Slash.Framework -exportPackage Assets %~dp0Slash.Framework.unitypackage -quit