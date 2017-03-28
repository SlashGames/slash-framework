@echo off

REM Setup
REM Install perl on your system, if not already existent: https://www.perl.org/get.html
REM Set environment variables:
REM UNITY_5_5_PATH is the path to your Unity3D 5.5 installation (e.g. C:\Program Files\Unity)

if ["%UNITY_5_5_PATH%"] == [""] (
	echo UNITY_5_5_PATH not set
	goto :EOF
)

BuildUnityPackage.bat "%UNITY_5_5_PATH%" "Debug Unity 5.5" "Slash.Framework.1.X.Unity.5.5" > BuildUnityPackage.log
