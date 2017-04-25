@echo off

REM Setup
REM Install perl on your system, if not already existent: https://www.perl.org/get.html
REM Set environment variables:
REM UNITY_5_3_PATH is the path to your Unity3D 5.5 installation (e.g. C:\Program Files\Unity)

if ["%UNITY_5_3_PATH%"] == [""] (
	echo UNITY_5_3_PATH not set
	goto :EOF
)

BuildUnityPackage.bat "%UNITY_5_3_PATH%" "Debug" "Slash.Framework.1.X.Unity.5.3" > BuildUnityPackage.log
