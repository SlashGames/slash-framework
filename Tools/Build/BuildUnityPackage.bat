REM Clean and build solution
%NET40_HOME%\MSBuild.exe ../../Source/Slash.Framework.sln /property:"Configuration=Debug NoLog4Net" /t:Clean;Build

REM Copy dlls from ECS
perl BuildUnityPackage.pl "../../Bin/Slash.ECS/AnyCPU/Debug"

REM Copy dlls from Unity.Common
perl BuildUnityPackage.pl "../../Bin/Slash.Unity.Common/Debug"

REM Build package
"%UNITY_PATH%Unity.exe" -batchmode -projectPath %~dp0Slash.Framework -exportPackage Assets %~dp0Slash.Framework.unitypackage -quit