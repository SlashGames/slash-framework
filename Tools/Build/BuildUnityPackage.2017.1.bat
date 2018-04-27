REM @echo off

REM Setup
REM Install perl on your system, if not already existent: https://www.perl.org/get.html
REM Set environment variables:
REM UNITY_2017_1_PATH is the path to your Unity3D 2017.1 installation (e.g. C:\Program Files\Unity)

if ["%UNITY_2017_1_PATH%"] == [""] (
	echo UNITY_2017_1_PATH not set
	goto :EOF
)

BuildUnityPackage.bat "%UNITY_2017_1_PATH%" "Debug" "Slash.Framework.1.X.Unity.2017.1"
